using System;
using System.Windows;
using System.Windows.Controls;
using TouristBusApp.Models;
using TouristBusApp.Resources;

namespace TouristBusApp.Windows
{
    public partial class AddTourPointWindow : Window
    {
        private readonly TourPoint _tourPoint;

        public AddTourPointWindow()
        {
            InitializeComponent();
            _tourPoint = new TourPoint();

            foreach (TourPoint tp in ProjectResource.Instance.TourPointsRep.Read())
            {
                var label = new Label
                {
                    Content = $"Стоимость дороги из и до {tp.Name}"
                };
                var textBox = new TextBox
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
                _tourPoint.Name = TourPointNameTextBox.Text;
                foreach (object? element in DistanceToOtherTourPointsStackPanel.Children)
                {
                    if (element is not TextBox tb) continue;
                    if (string.IsNullOrEmpty(tb.Text) || !int.TryParse(tb.Text, out _))
                        throw new Exception(
                            $"Необходимо задать дистанцию из и до {(tb.DataContext as TourPoint).Name}");
                }

                ProjectResource.Instance.TourPointsRep.Create(_tourPoint);

                foreach (object? element in DistanceToOtherTourPointsStackPanel.Children)
                {
                    if (element is not TextBox tb) continue;
                    ProjectResource.Instance.RoadsRep.Create(new Road
                    {
                        DepartureTourPointId = _tourPoint.Id,
                        ArrivalTourPointId = (tb.DataContext as TourPoint).Id,
                        Price = int.Parse(tb.Text)
                    });
                    ProjectResource.Instance.RoadsRep.Create(new Road
                    {
                        ArrivalTourPointId = _tourPoint.Id,
                        DepartureTourPointId = (tb.DataContext as TourPoint).Id,
                        Price = int.Parse(tb.Text)
                    });
                }

                DialogResult = true;
                Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
    }
}