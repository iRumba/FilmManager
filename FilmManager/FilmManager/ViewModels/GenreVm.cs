using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmManagerCore.Models;

namespace FilmManager.ViewModels
{
    public class GenreVm : ModelReflection<Genre>
    {
        string _name;

        public GenreVm() : base(new Genre()) { }
        public GenreVm(Genre source) : base(source) { }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        //public ObservableCollection<FilmVm> Films { get; set; }

        internal override Genre FillModel()
        {
            _source.Name = Name;
            _source.Changed += _source_Changed;
            //_source.Films = Films?.Select(f => f.GetModel()).ToList();
            return _source;
        }

        void _source_Changed(object sender, EventArgs e)
        {
            _source.Changed -= _source_Changed;
            Fill();
            _source.Changed += _source_Changed;
        }

        protected internal override void FillFromModel()
        {
            Fill();
            //var films = _source.Films?.Select(f => new FilmVm(f));
            //if (films != null)
            //    Films = new ObservableCollection<FilmVm>(films);
            //else
            //    Films = new ObservableCollection<FilmVm>();
        }

        void Fill()
        {
            Name = _source.Name;
        }
    }
}
