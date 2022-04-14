using System;
using System.Windows;
using System.Windows.Controls;
using TouristBusApp.Models;
using TouristBusApp.Resources;

namespace TouristBusApp.UserControls
{
    public partial class EditableTourPointControl : UserControl
    {
        private TourPoint _tourPoint;
        private bool HasChanges
        {
            get => TextBlockSaveRequired.IsVisible;
            set => TextBlockSaveRequired.Visibility = value ? Visibility.Visible : Visibility.Hidden;
        }
        public EditableTourPointControl(TourPoint tourPoint)
        {
            InitializeComponent();
            _tourPoint = tourPoint;

            
            TourPointNameTextBox.Text = _tourPoint.Name;
            
            HasChanges = false;
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _tourPoint.Name = TourPointNameTextBox.Text;
                ProjectResource.Instance.TourPointsRep.Update(_tourPoint);
                
                HasChanges = false;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void TourPointNameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            HasChanges = true;
        }
    }
}