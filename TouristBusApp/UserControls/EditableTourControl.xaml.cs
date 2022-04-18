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
            set
            {
                TextBlockSaveRequired.Visibility = value ? Visibility.Visible : Visibility.Hidden;
                if (SaveButton != null) SaveButton.IsEnabled = value;
                if (_tour != null) RefreshPriceAndRequestsCondition();
            }
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
            IEnumerable<TourPoint> tps = ProjectResource.Instance.TourPointsRep.Read();
            int index = 0;
            foreach (TourPoint point in
                     _tour.TourPointIds.Select(tpId => tps.FirstOrDefault(tp => tp.Id == tpId)))
            {
                ComboBox comboBox = CreateComboBoxForTourPoints();
                comboBox.SelectedItem = point.Name;
                comboBox.DataContext = index++;
                comboBox.SelectionChanged += TourPointComboBox_OnSelectionChanged;
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

            RefreshPriceAndRequestsCondition();
        }

        private void RefreshPriceAndRequestsCondition()
        {
            int price = 0;
            for (int i = 0; i < _tour.TourPointIds.Count() - 1; ++i)
                price += ProjectResource.Instance.RoadsRep.Read().Where(r =>
                    r.DepartureTourPointId == _tour.TourPointIds[i] &&
                    r.ArrivalTourPointId == _tour.TourPointIds[i + 1]).FirstOrDefault().Price;
            TextBlockPrice.Text = $"{price}$";
            
            
            TourRequestConditionTextBlock.Text = $"{ProjectResource.Instance.TourRequestsRep.Read().Count(tr=>tr.TourId == _tour.Id)}/{_tour.Bus.Capacity}";
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            HasChanges = true;
            _tour.TourPointIds.Remove((int) (sender as Button).DataContext);
            RefreshTourPoints();
            RefreshPriceAndRequestsCondition();
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

                if (ProjectResource.Instance.TourRequestsRep.Read().Any(tr => tr.TourId == _tour.Id) &&
                    MessageBox.Show("Изменение тура вызовет удаление всех заявок на этот тур", "Внимание!!!",
                        MessageBoxButton.OKCancel) != MessageBoxResult.OK) return;

                _tour.Name = TourNameTextBox.Text;
                _tour.Departure = (DateTime) DepartureDatePicker.SelectedDate;
                _tour.Arrival = (DateTime) ArrivalDatePicker.SelectedDate;

                foreach (TourRequest tourRequest in ProjectResource.Instance.TourRequestsRep.Read())
                    ProjectResource.Instance.TourRequestsRep.Delete(tourRequest.Id);

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
                RefreshPriceAndRequestsCondition();
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

        private void TourPointComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int tourPointIdsIndex = (int) comboBox.DataContext;
            _tour.TourPointIds[tourPointIdsIndex] = ProjectResource.Instance.TourPointsRep.Read()
                .FirstOrDefault(tp => tp.Name == comboBox.SelectedValue.ToString()).Id;
            HasChanges = true;
        }
    }
}