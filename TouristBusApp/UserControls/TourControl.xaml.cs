using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TouristBusApp.Models;
using TouristBusApp.Resources;

namespace TouristBusApp.UserControls
{
    public partial class TourControl : UserControl
    {
        private Tour _tour;
        private User _signedInUser;

        public TourControl(Tour tour)
        {
            InitializeComponent();

            _tour = tour;
            _signedInUser = ProjectResource.Instance.Authentication.GetSignedInUser();

            TourNameTextBox.Text = tour.Name;
            BusNumberTextBlock.Text = tour.Bus.Number;
            ArrivalDateDatePicker.SelectedDate = tour.Arrival;
            DepartureDateDatePicker.SelectedDate = tour.Departure;

            TourPointsStackPanel.Children.Clear();
            TextBlockPrice.Text = "-";
            IEnumerable<TourPoint> tps = ProjectResource.Instance.TourPointsRep.Read();
            foreach (TourPoint point in
                     tour.TourPointIds.Select(tpId => tps.FirstOrDefault(tp => tp.Id == tpId)))
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = point.Name;
                TourPointsStackPanel.Children.Add(textBlock);
            }

            int price = 0;
            for (int i = 0; i < tour.TourPointIds.Count - 1; ++i)
                price += ProjectResource.Instance.RoadsRep.Read().FirstOrDefault(r =>
                    r.DepartureTourPointId == tour.TourPointIds[i] &&
                    r.ArrivalTourPointId == tour.TourPointIds[i + 1]).Price;
            TextBlockPrice.Text = $"{price}$";


            User user = ProjectResource.Instance.Authentication.GetSignedInUser();
            SubscribeButton.Content =
                ProjectResource.Instance.TourRequestsRep.Read().Any(tr => tr.TourId == tour.Id && tr.UserId == user.Id)
                    ? "Отписаться"
                    : "Подписаться";
            TourRequestConditionTextBlock.Text =
                $"{ProjectResource.Instance.TourRequestsRep.Read().Count(tr => tr.TourId == tour.Id)}/{tour.Bus.Capacity}";
        }

        private void ButtonSubscribe_OnClick(object sender, RoutedEventArgs e)
        {

            if (ProjectResource.Instance.TourRequestsRep.Read()
                .Any(tr => tr.TourId == _tour.Id && tr.UserId == _signedInUser.Id))
            {
                foreach (TourRequest tr in ProjectResource.Instance.TourRequestsRep.Read()
                             .Where(tr => tr.TourId == _tour.Id && tr.UserId == _signedInUser.Id))
                {
                    ProjectResource.Instance.TourRequestsRep.Delete(tr.Id);
                }
                SubscribeButton.Content = "Подписаться";
            }
            else
            {
                ProjectResource.Instance.TourRequestsRep.Create(new TourRequest()
                {
                    TourId = _tour.Id,
                    UserId = _signedInUser.Id
                });
                SubscribeButton.Content = "Отписаться";
            }

            TourRequestConditionTextBlock.Text =
                $"{ProjectResource.Instance.TourRequestsRep.Read().Count(tr => tr.TourId == _tour.Id)}/{_tour.Bus.Capacity}";
        }
    }
}