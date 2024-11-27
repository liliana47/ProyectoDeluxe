using BLL;
using LiveCharts.Wpf.Charts.Base;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

namespace GUI.Pages
{
    /// <summary>
    /// Lógica de interacción para GraficaFacturas.xaml
    /// </summary>
    public partial class GraficaFacturas : UserControl
    {
        public SeriesCollection SeriesCollection { get; set; }
        public ObservableCollection<string> Periodos { get; set; }
        public ObservableCollection<string> PeriodoOptions { get; set; }
        public string SelectedPeriodo { get; set; }
        public LiveCharts.Wpf.Separator AxisYSeparator { get; set; }

        private FacturaBLL facturaBLL;

        public GraficaFacturas()
        {
            InitializeComponent();
            DataContext = this;

            facturaBLL = new FacturaBLL();
            PeriodoOptions = new ObservableCollection<string> { "Día", "Mes", "Año" };
            SelectedPeriodo = "Día";
            AxisYSeparator = new LiveCharts.Wpf.Separator { Step = 1 };

            LoadDataFromDatabase(SelectedPeriodo);
        }

        private void LoadDataFromDatabase(string periodo)
        {
            try
            {
                // Obtener los datos del BLL
                var facturasPorPeriodo = facturaBLL.ObtenerFacturasPorPeriodo(periodo);

                // Validar datos para evitar errores de rango
                if (facturasPorPeriodo == null || !facturasPorPeriodo.Values.Any())
                {
                    MessageBox.Show("No hay datos disponibles para el período seleccionado.");
                    return;
                }

                // Configurar SeriesCollection para el gráfico
                SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Facturas",
                        Values = new ChartValues<int>(facturasPorPeriodo.Values),
                        Stroke = new SolidColorBrush(Color.FromRgb(0, 120, 215)),
                        Fill = new SolidColorBrush(Color.FromArgb(50, 0, 120, 215))
                    }
                };

                Periodos = new ObservableCollection<string>(facturasPorPeriodo.Keys);

                // Actualizar el gráfico
                AdjustAxisYMaxValue();
                Chart.Series = SeriesCollection;
                Chart.AxisX[0].Labels = Periodos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}");
            }
        }

        private void AdjustAxisYMaxValue()
        {
            try
            {
                // Obtener el máximo de las series
                var maxFacturas = SeriesCollection
                    .SelectMany(series => series.Values.Cast<int>())
                    .DefaultIfEmpty(0)
                    .Max();

                // Configurar los límites del eje Y
                var yAxis = Chart.AxisY.FirstOrDefault();
                if (yAxis != null)
                {
                    yAxis.MaxValue = Math.Ceiling(maxFacturas / 5.0) * 5;
                    yAxis.MinValue = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al ajustar el eje Y: {ex.Message}");
            }
        }

        private void PeriodoSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PeriodoComboBox.SelectedItem != null)
            {
                string selectedPeriodo = PeriodoComboBox.SelectedItem.ToString();
                LoadDataFromDatabase(selectedPeriodo);
            }
        }
    }
}
