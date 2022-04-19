using System.Windows;
using TouristBusApp.Models;
using TouristBusApp.Resources;

namespace TouristBusApp.Windows
{
    public partial class AddTourPointFilterWindow : Window
    {
        public AddTourPointFilterWindow()
        {
            InitializeComponent();

            foreach (TourPoint tr in ProjectResource.Instance.TourPointsRep.Read())
            {
                TourPointFilterComboBox.Items.Add(tr);
                TourPointFilterComboBox.DisplayMemberPath = "Name";
            }
        }

        public TourPoint TourPoint { get; private set; }

        private void ButtonChoose_OnClick(object sender, RoutedEventArgs e)
        {
            TourPoint = TourPointFilterComboBox.SelectedValue as TourPoint;
            DialogResult = true;
            Close();
        }
    }
}