using System.Windows.Controls;
using TouristBusApp.Models;

namespace TouristBusApp.UserControls
{
    public partial class BusControl : UserControl
    {
        public BusControl(Bus bus)
        {
            InitializeComponent();

            BusNumberTextBlock.Text = bus.Number;
            BusCapacityTextBlock.Text = $"{bus.Capacity}";
        }
    }
}