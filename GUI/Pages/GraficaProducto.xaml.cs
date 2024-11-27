using BLL;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;

namespace GUI.Pages
{
    /// <summary>
    /// Lógica de interacción para GraficaProducto.xaml
    /// </summary>
    public partial class GraficaProducto : UserControl
    {
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }

        public GraficaProducto()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            try
            {
                var productoBLL = new ProductoBLL();
                var productosMasVendidos = productoBLL.ObtenerProductosMasVendidos();

                // Validar si hay datos
                if (productosMasVendidos == null || !productosMasVendidos.Any())
                {
                    MessageBox.Show("No hay datos de productos para mostrar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Extraer nombres y cantidades para el gráfico
                Labels = productosMasVendidos.Select(p => p.NombreProducto).ToList();
                var cantidades = productosMasVendidos.Select(p => p.CantidadVendida).ToList();

                // Crear el gráfico
                SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Productos",
                        Values = new ChartValues<int>(cantidades),
                        DataLabels = true
                    }
                };

                GraficaProductos.Series = SeriesCollection;
                GraficaProductos.AxisX[0].Labels = Labels;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al cargar los datos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
