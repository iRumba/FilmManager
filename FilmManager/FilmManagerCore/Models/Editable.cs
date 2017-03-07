using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmManagerCore.Models
{
    public class Editable
    {
        bool _editing;

        public event EventHandler Changed;

        public void BeginEdit()
        {
            _editing = true;
        }

        public void EndEdit()
        {
            _editing = false;
            OnChanged();
        }

        protected void OnChanged()
        {
            if (!_editing)
                Changed?.Invoke(this, new EventArgs());
        }
    }
}
