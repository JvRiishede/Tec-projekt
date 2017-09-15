using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Frontend.Annotations;

namespace Data
{
    public class OrderItem : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public bool ErDrink { get; set; }

        private string _name;

        public string Full
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Full));
            }
        }

        public void Combine()
        {
            Full = Name + "   " + "Antal: " + Amount.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
