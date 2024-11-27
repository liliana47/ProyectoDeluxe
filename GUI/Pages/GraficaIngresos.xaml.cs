using BLL;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GUI.Pages
{
    /// <summary>
    /// Lógica de interacción para GraficaIngresos.xaml
    /// </summary>
    public partial class GraficaIngresos : UserControl
    {
        private FacturaBLL facturaBLL = new FacturaBLL();

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        public GraficaIngresos()
        {
            InitializeComponent();
            UpdateChart();
        }

        private void UpdateChart()
        {
            try
            {
                double totalFacturas = facturaBLL.ObtenerTotalFacturas();

                // Validar si el valor es cero o negativo
                if (totalFacturas <= 0)
                {
                    MessageBox.Show("No hay datos de facturación para mostrar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Total Facturas",
                        Values = new ChartValues<double> { totalFacturas },
                        DataLabels = true,
                        LabelPoint = p => p.Y.ToString("C")
                    }
                };

                Labels = new[] { "Total" };
                Formatter = val => val.ToString("C");

                DataContext = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al cargar los datos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
