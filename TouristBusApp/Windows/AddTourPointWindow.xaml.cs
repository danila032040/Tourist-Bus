using System;
using System.Windows;
using System.Windows.Controls;
using TouristBusApp.Models;
using TouristBusApp.Resources;

namespace TouristBusApp.Windows
{
    public partial class AddTourPointWindow : Window
    {
        public TourPoint TourPoint { get; private set; }
        public AddTourPointWindow()
        {
            InitializeComponent();
            TourPoint = new TourPoint();

            foreach (TourPoint tp in ProjectResource.Instance.TourPointsRep.Read())
            {
                Label label = new Label
                {
                    Content = $"Стоимость дороги до {tp.Name}"
                };
                TextBox textBox = new TextBox()
                {
                    DataContext = tp
                };
                DistanceToOtherTourPointsStackPanel.Children.Add(label);
                DistanceToOtherTourPointsStackPanel.Children.Add(textBox);
            }
        }

        private void ButtonCreate_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var element in DistanceToOtherTourPointsStackPanel.Children)
                {
                    if (element is not TextBox tb) continue;
                    if (string.IsNullOrEmpty(tb.Text) || !int.TryParse(tb.Text, out _))
                        throw new Exception($"Необходимо задать дистанцию до {(tb.DataContext as TourPoint).Name}");
                }

                foreach (var element in DistanceToOtherTourPointsStackPanel.Children)
                {
                    if (element is not TextBox tb) continue;
                    ProjectResource.Instance.RoadsRep.Create(new Road
                    {
                        DepartureTourPointId = TourPoint.Id,
                        ArrivalTourPointId = (tb.DataContext as TourPoint).Id,
                        Price = int.Parse(tb.Text)
                    }); 
                    ProjectResource.Instance.RoadsRep.Create(new Road
                    {
                        ArrivalTourPointId = TourPoint.Id,
                        DepartureTourPointId = (tb.DataContext as TourPoint).Id,
                        Price = int.Parse(tb.Text)
                    }); 
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
    }
}