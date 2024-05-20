using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{

    public partial class MainWindow : Window
    {
        private const string PlanteApiUrl = "https://localhost:44346/api/Plante/GetPlantess";
        private const string InformationApiUrl = "https://localhost:44346/api/Information/GetInformations";
        private const string ReserveApiUrl = "https://localhost:44346/api/reserve/GetReservess";
        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task<(int? idPlante, string nomPlante)> GetIdAndNomPlanteFromApi(string planteNom)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(PlanteApiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var plantes = JsonConvert.DeserializeObject<List<Plante>>(json);
                        var plante = plantes.Find(p => p.nomPlante.ToLower() == planteNom);
                        return (plante?.idPlante, plante?.nomPlante);
                    }
                    else
                    {
                        return (null, null);
                    }
                }
            }
            catch (Exception)
            {
                return (null, null);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
           

            string planteNom = planteTextBox.Text.ToLower();
            string stade = stadeTextBox.Text.ToLower();

            if (string.IsNullOrWhiteSpace(planteNom) || string.IsNullOrWhiteSpace(stade))
            {
                errorTextBlock.Text = "Veuillez saisir un nom de plante et un stade.";
                errorTextBlock.Visibility = Visibility.Visible;
                return;
            }

            errorTextBlock.Visibility = Visibility.Collapsed;

            var (idPlante, nomPlante) = await GetIdAndNomPlanteFromApi(planteNom);

            if (idPlante == null)
            {
                MessageBox.Show("Aucune plante trouvée avec ce nom.");
                return;
            }

            if (string.IsNullOrWhiteSpace(nomPlante))
            {
                MessageBox.Show("Impossible de récupérer le nom de la plante.");
                return;
            }

            plante.Text = nomPlante;

            var kc = await GetKcForStadeFromApi(idPlante, stade);

            if (string.IsNullOrWhiteSpace(kc))
            {
                MessageBox.Show("Aucune information trouvée pour le stade spécifié.");
                return;
            }

            kcTextBlock.Text = kc;

            var codePostale = codepostale.Text;

            var reserveDeau = await GetReserveDeauFromApi(codePostale);

            if (!reserveDeau.HasValue)
            {
                MessageBox.Show("Aucune information trouvée pour le code postal spécifié.");
                return;
            }

            reservedeau.Text = reserveDeau.Value.ToString();
            kc = kc.Replace('.', ',');
            double surface;
            if (!double.TryParse(surfacemm2.Text, out surface) || surface == 0)
            {
                MessageBox.Show("Veuillez entrer une valeur valide et non nulle pour la surface.");
                return;
            }

            double calcul = 0.5 * surface;

            if (!double.TryParse(kc, out double kcValue) || calcul == 0 || kcValue == 0)
            {
                MessageBox.Show("Impossible d'effectuer le calcul. Assurez-vous que le coefficient kc et la surface sont des valeurs valides et que la surface n'est pas égale à zéro.");
                return;
            }

            double resultatFinal = (double)reserveDeau.Value / calcul * kcValue;



            resultTextBlock.Text = resultatFinal.ToString();

        }

        private async Task<WeatherData> GetWeatherDataByInsee(string insee)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string apiUrl = $"https://localhost:7205/api/Meteo/GetMeteoByInsee?insee={insee}";
                    var response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var weatherData = JsonConvert.DeserializeObject<WeatherData>(json);
                        return weatherData;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<string> GetInseeFromPostalCode(string postalCode)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string apiUrl = $"https://api.meteo-concept.com/api/location/cities?token=94c216792ecd05dfcb72e8e4662ed43cfbd07953164ee576627356b23cf8a3d8&search={postalCode}";
                    var response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var cityResponse = JsonConvert.DeserializeObject<CityResponse>(json);
                        if (cityResponse.Cities != null && cityResponse.Cities.Count > 0)
                        {
                            return cityResponse.Cities[0].Insee;
                        }
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<string> GetKcForStadeFromApi(int? idPlante, string stade)
        {
            try
            {
                if (!idPlante.HasValue)
                    return null;

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(InformationApiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var informations = JsonConvert.DeserializeObject<List<Information>>(json);
                        var information = informations.Find(i => i.idPlante == idPlante && i.stades.ToLower() == stade);
                        return information?.kc;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<int?> GetReserveDeauFromApi(string codePostale)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codePostale))
                    return null;

                if (!int.TryParse(codePostale, out int codePostalInt))
                    return null;

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(ReserveApiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var reserves = JsonConvert.DeserializeObject<List<Reserve>>(json);
                        var reserve = reserves.Find(r => r.codePostale == codePostalInt);
                        return reserve?.reserveDeau;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    public class Plante
    {
        public int idPlante { get; set; }
        public string nomPlante { get; set; }
    }

    public class Information
    {
        public string kc { get; set; }
        public string stades { get; set; }
        public int idPlante { get; set; }
    }

    public class Reserve
    {
        public int idReserve { get; set; }
        public int codePostale { get; set; }
        public int reserveDeau { get; set; }
    }

    public class CityResponse
    {
        [JsonProperty("cities")]
        public List<CityItem> Cities { get; set; }
    }

    public class CityItem
    {
        [JsonProperty("insee")]
        public string Insee { get; set; }

        [JsonProperty("cp")]
        public string Cp { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [JsonProperty("altitude")]
        public string Altitude { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }


    public class WeatherData
    {
        public City city { get; set; }
        public DateTime update { get; set; }
        public List<Forecast> forecast { get; set; }
    }

    public class City
    {
        public string insee { get; set; }
        public int cp { get; set; }
        public string name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int altitude { get; set; }
    }

    public class Forecast
    {
        public string insee { get; set; }
        public int cp { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int day { get; set; }
        public DateTime datetime { get; set; }
        public int wind10m { get; set; }
        public int gust10m { get; set; }
        public int dirwind10m { get; set; }
        public double rr10 { get; set; }
        public double rr1 { get; set; }
        public int probarain { get; set; }
        public int weather { get; set; }
        public int tmin { get; set; }
        public int tmax { get; set; }
        public int sun_hours { get; set; }
        public int etp { get; set; }
        public int probafrost { get; set; }
        public int probafog { get; set; }
        public int probawind70 { get; set; }
        public int probawind100 { get; set; }
        public int gustx { get; set; }
    }


}
