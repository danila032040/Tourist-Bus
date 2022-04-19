using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using TouristBusApp.Models;
using TouristBusApp.Resources;
using TouristBusApp.UserControls;

namespace TouristBusApp.Windows
{
    public partial class AdminUserWindow : Window
    {
        public AdminUserWindow()
        {
            InitializeComponent();
        }

        private void ButtonSignOut_OnClick(object sender, RoutedEventArgs e)
        {
            ProjectResource.Instance.Authentication.SignOut();
            Application.Current.MainWindow = new AuthenticationWindow();
            Application.Current.MainWindow.Show();
            Close();
        }

        private void ButtonAddTour_OnClick(object sender, RoutedEventArgs e)
        {
            ProjectResource.Instance.ToursRep.Create(new Tour
            {
                Name = "Test",
                Bus = null,
                TourPointIds = null,
                Departure = DateTime.Now,
                Arrival = DateTime.Now.AddDays(1)
            });
            RefreshTourStackPanel();
        }

        private void ButtonAddTourPoint_OnClick(object sender, RoutedEventArgs e)
        {
            Window window = new AddTourPointWindow();
            window.Owner = this;
            if (window.ShowDialog() != true) return;

            RefreshTourPointsPanel();
        }


        private void RefreshTourStackPanel()
        {
            ToursStackPanel.Children.Clear();
            foreach (Tour tour in ProjectResource.Instance.ToursRep.Read())
            {
                var deleteButton = new Button
                {
                    Content = "X"
                };
                deleteButton.DataContext = tour.Id;
                deleteButton.Click += DeleteTourButton_OnClick;
                var sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(5, 5, 5, 50)
                };
                sp.Children.Add(new EditableTourControl(tour));
                sp.Children.Add(deleteButton);
                ToursStackPanel.Children.Add(sp);
            }
        }

        private void RefreshBusStackPanel()
        {
            BusStackPanel.Children.Clear();
            foreach (Bus bus in ProjectResource.Instance.BussesRep.Read())
            {
                var sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(5, 5, 5, 50)
                };
                sp.Children.Add(new BusControl(bus));
                BusStackPanel.Children.Add(sp);
            }
        }

        private void RefreshTourPointsPanel()
        {
            TourPointsStackPanel.Children.Clear();
            foreach (TourPoint tourPoint in ProjectResource.Instance.TourPointsRep.Read())
            {
                var deleteButton = new Button
                {
                    Content = "X"
                };
                deleteButton.DataContext = tourPoint.Id;
                deleteButton.Click += DeleteTourPointButton_OnClick;
                var sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(5, 5, 5, 50)
                };
                sp.Children.Add(new EditableTourPointControl(tourPoint));
                sp.Children.Add(deleteButton);
                TourPointsStackPanel.Children.Add(sp);
            }
        }

        private void RefreshRoadsTable()
        {
            RoadsTable.Columns.Clear();
            RoadsTable.RowGroups.Clear();
            RoadsTable.RowGroups.Add(new TableRowGroup());
            RoadsTable.Columns.Add(new TableColumn());
            RoadsTable.Columns.Add(new TableColumn());
            IEnumerable<TourPoint> tps = ProjectResource.Instance.TourPointsRep.Read();
            IEnumerable<Road> rds = ProjectResource.Instance.RoadsRep.Read();

            RoadsTable.RowGroups[0].Rows.Add(new TableRow());
            RoadsTable.RowGroups[0].Rows.Add(new TableRow());
            RoadsTable.RowGroups[0].Rows.Add(new TableRow());
            RoadsTable.RowGroups[0].Rows[0].Cells.Add(new TableCell {ColumnSpan = 2});
            RoadsTable.RowGroups[0].Rows[1].Cells.Add(new TableCell());
            RoadsTable.RowGroups[0].Rows[1].Cells.Add(new TableCell(new Paragraph(new Run("Точки"))
                {TextAlignment = TextAlignment.Center}));
            RoadsTable.RowGroups[0].Rows[0].Cells
                .Add(new TableCell(new Paragraph(new Run("Точки прибытия")) {TextAlignment = TextAlignment.Center})
                    {ColumnSpan = tps.Count(), Background = Brushes.Cyan});
            RoadsTable.RowGroups[0].Rows[2].Cells
                .Add(new TableCell(new Paragraph(new Run("Точки отправления")) {TextAlignment = TextAlignment.Center})
                    {RowSpan = tps.Count(), Background = Brushes.Cyan});
            int indexRow = 2;
            int indexColumn = 2;
            foreach (TourPoint tp in tps)
            {
                RoadsTable.Columns.Add(new TableColumn());
                RoadsTable.RowGroups[0].Rows[indexRow].Cells.Add(
                    new TableCell(new Paragraph(new Run(tp.Name)) {TextAlignment = TextAlignment.Center})
                        {Background = Brushes.Gray});
                for (int i = 0; i < tps.Count(); ++i)
                    RoadsTable.RowGroups[0].Rows[indexRow].Cells.Add(new TableCell(new Paragraph(new Run("-"))
                        {TextAlignment = TextAlignment.Center}));
                RoadsTable.RowGroups[0].Rows[1].Cells.Add(
                    new TableCell(new Paragraph(new Run(tp.Name)) {TextAlignment = TextAlignment.Center})
                        {Background = Brushes.Azure});

                indexRow++;
                indexColumn++;
                RoadsTable.RowGroups[0].Rows.Add(new TableRow());
            }

            RoadsTable.RowGroups[0].Rows.RemoveAt(RoadsTable.RowGroups[0].Rows.Count - 1);

            int n = tps.Count();
            int m = tps.Count();
            for (int j = 3; j < m + 2; ++j)
            {
                Road road = rds.FirstOrDefault(r =>
                    r.DepartureTourPointId == tps.ElementAt(0).Id &&
                    r.ArrivalTourPointId == tps.ElementAt(j - 2).Id);
                RoadsTable.RowGroups[0].Rows[2].Cells[j].Blocks.Clear();
                var txtBlock = new TextBox
                {
                    TextAlignment = TextAlignment.Center,
                    Text = road.Price.ToString(),
                    DataContext = road
                };
                RoadsTable.RowGroups[0].Rows[2].Cells[j].Blocks.Add(new BlockUIContainer(txtBlock));
            }

            for (int i = 3; i < n + 2; ++i)
                for (int j = 1; j < m + 1; ++j)
                    if (i != j + 1)
                    {
                        Road road = rds.FirstOrDefault(r =>
                            r.DepartureTourPointId == tps.ElementAt(i - 2).Id &&
                            r.ArrivalTourPointId == tps.ElementAt(j - 1).Id);
                        RoadsTable.RowGroups[0].Rows[i].Cells[j].Blocks.Clear();
                        var txtBlock = new TextBox
                        {
                            TextAlignment = TextAlignment.Center,
                            Text = road.Price.ToString(),
                            DataContext = road
                        };
                        RoadsTable.RowGroups[0].Rows[i].Cells[j].Blocks.Add(new BlockUIContainer(txtBlock));
                    }
        }

        private void SaveRoadsButton_OnClick(object sender, RoutedEventArgs e)
        {
            IEnumerable<TourPoint> tps = ProjectResource.Instance.TourPointsRep.Read();
            IEnumerable<Road> rds = ProjectResource.Instance.RoadsRep.Read();

            int n = tps.Count();
            int m = tps.Count();

            for (int j = 3; j < m + 2; ++j)
            {
                Road road = rds.FirstOrDefault(r =>
                    r.DepartureTourPointId == tps.ElementAt(0).Id &&
                    r.ArrivalTourPointId == tps.ElementAt(j - 2).Id);
                road.Price = Convert.ToInt32(
                    ((RoadsTable.RowGroups[0].Rows[2].Cells[j].Blocks.FirstBlock as BlockUIContainer).Child as TextBox)
                    .Text);
                ProjectResource.Instance.RoadsRep.Update(road);
            }

            for (int i = 3; i < n + 2; ++i)
                for (int j = 1; j < m + 1; ++j)
                    if (i != j + 1)
                    {
                        Road road = rds.FirstOrDefault(r =>
                            r.DepartureTourPointId == tps.ElementAt(i - 2).Id &&
                            r.ArrivalTourPointId == tps.ElementAt(j - 1).Id);
                        road.Price = Convert.ToInt32(
                            ((RoadsTable.RowGroups[0].Rows[i].Cells[j].Blocks.FirstBlock as BlockUIContainer)
                                .Child as TextBox)
                            .Text);
                        ProjectResource.Instance.RoadsRep.Update(road);
                    }

            RefreshRoadsTable();
        }

        private void DeleteTourButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удаление тура вызовет удаление всех заявок на этот тур", "Внимание!!!",
                    MessageBoxButton.OKCancel) != MessageBoxResult.OK) return;

            int tourId = (int) (sender as Button).DataContext;
            ProjectResource.Instance.ToursRep.Delete(tourId);
            foreach (TourRequest tourRequest in ProjectResource.Instance.TourRequestsRep.Read()
                         .Where(tr => tr.TourId == tourId))
                ProjectResource.Instance.TourRequestsRep.Delete(tourRequest.Id);

            RefreshTourStackPanel();
        }

        private void DeleteTourPointButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Будут удалены и дороги выходящие и входящие в эту точку!", "Внимание!!!",
                    MessageBoxButton.OKCancel) != MessageBoxResult.OK) return;

            int tourPointId = (int) (sender as Button).DataContext;
            ProjectResource.Instance.TourPointsRep.Delete(tourPointId);
            foreach (Road road in ProjectResource.Instance.RoadsRep.Read().Where(r =>
                         r.ArrivalTourPointId == tourPointId || r.DepartureTourPointId == tourPointId))
                ProjectResource.Instance.RoadsRep.Delete(road.Id);

            RefreshTourPointsPanel();
        }

        private void TourTabItem_OnSelected(object sender, RoutedEventArgs e)
        {
            RefreshTourStackPanel();
        }

        private void BussesTabItem_OnSelected(object sender, RoutedEventArgs e)
        {
            RefreshBusStackPanel();
        }

        private void TourPointsTabItem_OnSelected(object sender, RoutedEventArgs e)
        {
            RefreshTourPointsPanel();
        }

        private void RoadsTabItem_OnSelected(object sender, RoutedEventArgs e)
        {
            RefreshRoadsTable();
        }
    }
}