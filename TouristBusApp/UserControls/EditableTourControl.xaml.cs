using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using TouristBusApp.Models;
using TouristBusApp.Resources;

namespace TouristBusApp.UserControls
{
    public partial class EditableTourControl : UserControl
    {
        private Tour _tour;

        private bool HasChanges
        {
            get => TextBlockSaveRequired.IsVisible;
            set => TextBlockSaveRequired.Visibility = value ? Visibility.Visible : Visibility.Hidden;
        }

        public EditableTourControl(Tour tour)
        {
            InitializeComponent();

            _tour = tour;
            TourNameTextBox.Text = tour.Name;
            BusNumberComboBox.Items.Add("Не выбран");
            foreach (string busNumber in ProjectResource.Instance.BussesRep.Read().Select(b => b.Number))
            {
                BusNumberComboBox.Items.Add(busNumber);
            }

            if (tour.Bus != null) BusNumberComboBox.SelectedItem = tour.Bus.Number;
            else BusNumberComboBox.SelectedIndex = 0;

            DepartureDatePicker.SelectedDate = tour.Departure;
            ArrivalDatePicker.SelectedDate = tour.Arrival;

            RefreshTourPoints();


            HasChanges = false;
        }

        private void RefreshTourPoints()
        {
            TourPointsStackPanel.Children.Clear();
            TextBlockPrice.Text = "-";
            if (_tour.TourPointIds == null) return;
            foreach (TourPoint point in ProjectResource.Instance.TourPointsRep.Read()
                         .Where(tp => _tour.TourPointIds.Contains(tp.Id)))
            {
                ComboBox comboBox = CreateComboBoxForTourPoints();
                comboBox.SelectedItem = point.Name;
                Button deleteButton = new Button
                {
                    Content = "X"
                };
                deleteButton.DataContext = point.Id;
                deleteButton.Click += DeleteButton_OnClick;
                StackPanel sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                sp.Children.Add(comboBox);
                sp.Children.Add(deleteButton);
                TourPointsStackPanel.Children.Add(sp);
            }

            int price = 0;
            for (int i = 0; i < _tour.TourPointIds.Count() - 1; ++i)
                price += ProjectResource.Instance.RoadsRep.Read().Where(r =>
                    r.DepartureTourPointId == _tour.TourPointIds[i] &&
                    r.ArrivalTourPointId == _tour.TourPointIds[i + 1]).FirstOrDefault().Price;
            TextBlockPrice.Text = $"{price}$";
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            HasChanges = true;
            _tour.TourPointIds.Remove((int) (sender as Button).DataContext);
            RefreshTourPoints();
        }

        private ComboBox CreateComboBoxForTourPoints()
        {
            ComboBox res = new ComboBox();
            foreach (string tourPointName in ProjectResource.Instance.TourPointsRep.Read().Select(a => a.Name))
                res.Items.Add(tourPointName);
            return res;
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _tour.Name = TourNameTextBox.Text;
                if (BusNumberComboBox.SelectedIndex != 0)
                {
                    if (ProjectResource.Instance.ToursRep.Read().Any(t =>
                            t.Id != _tour.Id &&
                            t.Bus != null &&
                            t.Bus.Number == (string) BusNumberComboBox.SelectedItem &&
                            t.Departure <= _tour.Arrival &&
                            t.Arrival >= _tour.Departure))
                        throw new Exception("Этот автобус уже занят другим туром в этот период времени.");

                    _tour.Bus = ProjectResource.Instance.BussesRep.Read()
                        .FirstOrDefault(b => b.Number == (string) BusNumberComboBox.SelectedItem);
                }
                else _tour.Bus = null;

                if (DepartureDatePicker.SelectedDate == null)
                    throw new Exception("Необходимо выбрать дату отправления!!!");
                if (ArrivalDatePicker.SelectedDate == null)
                    throw new Exception("Необходимо выбрать дату возвращения!!!");
                if (ArrivalDatePicker.SelectedDate <= DepartureDatePicker.SelectedDate)
                    throw new Exception("Дата отправления и возвращения заданы некорректно!");

                _tour.Departure = (DateTime) DepartureDatePicker.SelectedDate;
                _tour.Arrival = (DateTime) ArrivalDatePicker.SelectedDate;

                ProjectResource.Instance.ToursRep.Update(_tour);
                HasChanges = false;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void ButtonAddTourPoint_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _tour.TourPointIds ??= new List<int>();
                TourPoint notUsed = ProjectResource.Instance.TourPointsRep.Read()
                    .Where(t => !_tour.TourPointIds.Contains(t.Id))
                    .FirstOrDefault();
                if (notUsed == null) throw new Exception("Все возможные точки тура уже добавлены!");
                _tour.TourPointIds.Add(notUsed.Id);
                RefreshTourPoints();
                HasChanges = true;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void TourNameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            HasChanges = true;
        }

        private void BusNumberComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HasChanges = true;
        }

        private void DepartureDatePicker_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
        {
            HasChanges = true;
        }

        private void ArrivalDatePicker_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
        {
            HasChanges = true;
        }
    }
}