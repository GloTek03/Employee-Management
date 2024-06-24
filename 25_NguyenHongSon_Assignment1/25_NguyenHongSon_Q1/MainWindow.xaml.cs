using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using _25_NguyenHongSon_Q1.Models;
using Microsoft.EntityFrameworkCore;

namespace _25_NguyenHongSon_Q1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PRN221_Asm1Context context;
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            resetTextbox();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            
            if (!ValidatePhone(txtPhone.Text))
            {
                MessageBox.Show("Invalid phone number! Please enter a valid 10-digit phone number starting with 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!ValidateIdNumber(txtIdNumber.Text))
            {
                MessageBox.Show("Invalid ID number! Please enter a valid ID number start with 5", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var newEmployee = new Employee
            {
                Name = txtEmployeeName.Text,
                Gender = Male.IsChecked == true ? "Male" : "Female",
                Dob = dob.SelectedDate.Value,
                Phone = txtPhone.Text,
                Idnumber = txtIdNumber.Text,
            };
            context.Employees.Add(newEmployee);
            context.SaveChanges();
            LoadData();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            context = new PRN221_Asm1Context();
            try
            {
                if (dataGridView.SelectedItem != null)
                {
                    Employee selectedEmployee = dataGridView.SelectedItem as Employee;
                    if (selectedEmployee != null)
                    {
                        if (!ValidatePhone(txtPhone.Text))
                        {
                            MessageBox.Show("Invalid phone number! Please enter a valid 10-digit phone number starting with 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (!ValidateIdNumber(txtIdNumber.Text))
                        {
                            MessageBox.Show("Invalid ID number! Please enter a valid ID number consisting digits.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        selectedEmployee.Name = txtEmployeeName.Text;
                        selectedEmployee.Gender = Male.IsChecked == true ? "Male" : "Female";
                        selectedEmployee.Dob = dob.SelectedDate.Value;
                        selectedEmployee.Phone = txtPhone.Text;
                        selectedEmployee.Idnumber = txtIdNumber.Text;
                        using(TransactionScope trans = new TransactionScope(TransactionScopeOption.Required))
                        {
                            try
                            {
                                context.Employees.Update(selectedEmployee);
                                context.SaveChanges();
                                trans.Complete();
                                MessageBox.Show("You complete to Edit");
                               
                            }
                            catch(Exception ex)
                            {
                                trans.Dispose();
                                Console.WriteLine(ex.ToString());
                            }
                            finally {
                                
                            }
                        }
                        LoadData();
                    }
                    
                }
                else
                {
                    MessageBox.Show("You need choose a people from DataGridView.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            context = new PRN221_Asm1Context();
            if(dataGridView.SelectedValue != null)
            {
                Employee employee = dataGridView.SelectedItem as Employee;
                try
                {
                    MessageBoxResult result = MessageBox.Show("Do you want to Delete?", "Delete Selected", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        using (TransactionScope trans = new TransactionScope(TransactionScopeOption.Required))
                        {
                            Employee empToDel = context.Employees.Find(employee.Id);
                            if (empToDel != null)
                            {
                                {
                                    try
                                    {
                                        context.Employees.Remove(empToDel);
                                        context.SaveChanges();
                                        
                                        MessageBox.Show("You complete to delete.");
                                        resetTextbox();
                                        
                                        trans.Complete();
                                    }
                                    catch (Exception ex)
                                    {
                                        trans.Dispose();
                                        MessageBox.Show(ex.ToString());
                                    }
                                    finally
                                    {
                                    }
                                }
                            }
                        }
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("You do not want to delete employee with ID: " + employee.Id);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("You need choose a employee.");
            }
            
        }

        private void dataGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridView.SelectedItem != null)
            {
                Employee selectedEmployee = dataGridView.SelectedItem as Employee;
                if (selectedEmployee != null)
                {
                    txtEmployeeId.Text = selectedEmployee.Id.ToString();
                    txtEmployeeName.Text = selectedEmployee.Name.ToString();
                    if(selectedEmployee.Gender == "Male")
                    {
                        Male.IsChecked = true;
                    }else if(selectedEmployee.Gender == "Female")
                    {
                        FeMale.IsChecked = true;
                    }
                    dob.Text = selectedEmployee.Dob.ToString();
                    txtPhone.Text = selectedEmployee.Phone.ToString();
                    txtIdNumber.Text = selectedEmployee.Idnumber.ToString();
                }
            }
        }
 
        private void LoadData()
        {
            context = new PRN221_Asm1Context();
            context.Database.OpenConnection();
            IEnumerable<Employee> query = from emp in context.Employees
                        select new Employee{ Id = emp.Id, Name = emp.Name, Gender = emp.Gender, Dob = emp.Dob, Phone = emp.Phone, Idnumber = emp.Idnumber };
            dataGridView.ItemsSource = query.ToList();
            context.Database.CloseConnection();
        }
        private void resetTextbox()
        {
            txtEmployeeId.Text = string.Empty;
            txtEmployeeName.Text = string.Empty;
            Male.IsChecked = true;
            Male.IsChecked = false;
            dob.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtIdNumber.Text = string.Empty;
        }
        private bool ValidatePhone(string phoneNumber)
        {
            Regex regex = new Regex(@"^0\d{9}$");
            return regex.IsMatch(phoneNumber);
        }
        private bool ValidateIdNumber(string idNumber)
        {
            
            Regex regex = new Regex(@"^5\d{1,5}$");
            return regex.IsMatch(idNumber);
            /*return !string.IsNullOrEmpty(idNumber) && long.TryParse(idNumber,out _);*/
        }
        

    }   
}
