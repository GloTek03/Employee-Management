using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _25_NguyenHongSon_Q2
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Star> stars = new List<Star>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddToListButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if any field is null or empty
            if (string.IsNullOrWhiteSpace(txtStarName.Text) ||
                !Dob.SelectedDate.HasValue ||
                string.IsNullOrWhiteSpace(txtDescription.Text) ||
                string.IsNullOrWhiteSpace(txtNationality.Text) ||
                !IsMaleCheckBox.IsChecked.HasValue)
            {
                MessageBox.Show("Please fill all fields!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Create a new Star object from the input fields
            Star newStar = new Star
            {
                StarName = txtStarName.Text,
                Dob = Dob.SelectedDate ?? DateTime.Now,
                Description = txtDescription.Text,
                IsMale = IsMaleCheckBox.IsChecked ?? false,
                Nationality = txtNationality.Text
            };

            // Add the new star to the list
            stars.Add(newStar);

            // Update the ListBox
            StarListBox.Items.Add("Name: " + newStar.StarName + " | Dob: " + newStar.Dob + " | Description: " + newStar.Description + " | Is Male: " + newStar.IsMale + " | Nationality: " + newStar.Nationality);
        }

        private async void SendToServerButton_Click(object sender, RoutedEventArgs e)
        {
            if (stars.Count == 0)
            {
                MessageBox.Show("There are no stars to send!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string jsonString = JsonSerializer.Serialize(stars, new JsonSerializerOptions { WriteIndented = true });    
            using(var client = new HttpClient())
            {
                /*var uri = new Uri("http://127.0.0.1:5000/stars");
                var res = await SendMessageToServer("127.0.0.1", 5000, jsonString);
                var content = new StringContent(jsonString,Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, content);*/
                var response = await SendMessageToServer("127.0.0.1", 5000, jsonString);
                if (response == "accepted")
                {
                    MessageBox.Show("Stars sent to server successfully.", "Success", MessageBoxButton.OK,MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("An error occured while sending stars to the server.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            static async Task<string> SendMessageToServer(string host, int port, string message)
            {
                try
                {
                    TcpClient client = new TcpClient(host, port);
                    NetworkStream stream = client.GetStream();

                    byte[] data = Encoding.UTF8.GetBytes(message);
                    await stream.WriteAsync(data, 0, data.Length);
                    Console.WriteLine("Message sent to server.");

                    // Receive response from server
                    data = new byte[1024];
                    int bytesRead = await stream.ReadAsync(data, 0, data.Length);
                    string response = Encoding.UTF8.GetString(data, 0, bytesRead);
                    Console.WriteLine($"Response from server: {response}");

                    stream.Close();
                    client.Close();
                    return "accepted";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return "error";
                }
            }
        }
    }
}
