using System.ComponentModel;

namespace LOFit.Models.Menu
{
    public class ProductListModel : INotifyPropertyChanged
    {
        private ProductModel _product;
        public ProductModel Product
        {
            get => _product;
            set
            {
                if (_product == value) return;

                _product = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Product"));
            }
        }

        public bool ButtonOk { get; set; }
        public bool ButtonNo { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
