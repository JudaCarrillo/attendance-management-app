using Xamarin.Forms;

namespace attendance_management_app.Utils.TemplatesSelector
{
    public class SelectedItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate SelectedTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (container is CollectionView collectionView)
            {
                var selectedItem = collectionView.SelectedItem;
                return selectedItem == item ? SelectedTemplate : DefaultTemplate;
            }
            return DefaultTemplate;
        }
    }
}