using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmManager.ViewModels;
using FilmManagerCore.Filters;
using FilmManagerCore.Models;

namespace FilmManager.ViewModels
{
    public class FilmFiltersSet : FilterSet
    {
        const string GENRES_FILTER_NAME = "genres";
        const string YEARS_FILTER_NAME = "years";
        const string SELFRATINGS_FILTER_NAME = "selfratings";
        const string RATINGS_FILTER_NAME = "ratings";

        public event EventHandler<GenreChangedEventArgs> GenreChanged;
        public event EventHandler<YearChangedEventArgs> YearChanged;
        public event EventHandler<SelfRatingChangedEventArgs> SelfRatingChanged;
        public event EventHandler<RatingChangedEventArgs> RatingChanged;



        public FilmFiltersSet()
        {
            this[GENRES_FILTER_NAME] = new FilterDataVm<GenreVm, long?>(g => g?.FillModel()?.GenreId, g => g.Name, "Жанр");
            this[YEARS_FILTER_NAME] = new FilterDataVm<int, int?>(y => y, y => y.ToString(), "Год");
            this[RATINGS_FILTER_NAME] = new FilterDataVm<float, float?>(r => r, r => $"Больше {r.ToString("F0")}", "Рейтинг");
            this[SELFRATINGS_FILTER_NAME] = new FilterDataVm<int, int?>(r => r, r => $"Нравится {r}", "Нравится");
            this[GENRES_FILTER_NAME].SelectedValueChanged += FilmFiltersSet_SelectedValueChanged;
            this[YEARS_FILTER_NAME].SelectedValueChanged += FilmFiltersSet_SelectedValueChanged;
            this[RATINGS_FILTER_NAME].SelectedValueChanged += FilmFiltersSet_SelectedValueChanged;
            this[SELFRATINGS_FILTER_NAME].SelectedValueChanged += FilmFiltersSet_SelectedValueChanged;
        }

        void FilmFiltersSet_SelectedValueChanged(object sender, SelectedValueChangedEventArgs e)
        {
            if (e.Source == this[GENRES_FILTER_NAME])
                GenreChanged?.Invoke(this, new GenreChangedEventArgs(e));
            else if (e.Source == this[YEARS_FILTER_NAME])
                YearChanged?.Invoke(this, new YearChangedEventArgs(e));
            else if (e.Source == this[RATINGS_FILTER_NAME])
                RatingChanged?.Invoke(this, new RatingChangedEventArgs(e));
            else if (e.Source == this[SELFRATINGS_FILTER_NAME])
                SelfRatingChanged?.Invoke(this, new SelfRatingChangedEventArgs(e));
        }

        public FilterDataVm<Genre, long?> Genres
        {
            get
            {
                return (FilterDataVm<Genre, long?>)this[GENRES_FILTER_NAME];
            }
        }

        public FilterDataVm<int, int?> Years
        {
            get
            {
                return (FilterDataVm<int, int?>)this[YEARS_FILTER_NAME];
            }
        }

        public FilterDataVm<int, int?> SelfRatings
        {
            get
            {
                return (FilterDataVm<int, int?>)this[SELFRATINGS_FILTER_NAME];
            }
        }

        public FilterDataVm<float, float?> Ratings
        {
            get
            {
                return (FilterDataVm<float, float?>)this[RATINGS_FILTER_NAME];
            }
        }
    }

    public class GenreChangedEventArgs : SelectedValueChangedEventArgs
    {
        public new FilterDataVm<GenreVm, long?> Source
        {
            get
            {
                return (FilterDataVm<GenreVm, long?>)base.Source;
            }
        }

        public new long? NewValue
        {
            get
            {
                return (long?)base.NewValue;
            }
        }

        public GenreChangedEventArgs(SelectedValueChangedEventArgs e) : base(e.Source, e.NewValue) { }
    }

    public class YearChangedEventArgs : SelectedValueChangedEventArgs
    {
        public new FilterDataVm<int, int?> Source
        {
            get
            {
                return (FilterDataVm<int, int?>)base.Source;
            }
        }

        public new int? NewValue
        {
            get
            {
                return (int?)base.NewValue;
            }
        }

        public YearChangedEventArgs(SelectedValueChangedEventArgs e) : base(e.Source, e.NewValue) { }
    }

    public class SelfRatingChangedEventArgs : SelectedValueChangedEventArgs
    {
        public new FilterDataVm<int, int?> Source
        {
            get
            {
                return (FilterDataVm<int, int?>)base.Source;
            }
        }

        public new int? NewValue
        {
            get
            {
                return (int?)base.NewValue;
            }
        }

        public SelfRatingChangedEventArgs(SelectedValueChangedEventArgs e) : base(e.Source, e.NewValue) { }
    }

    public class RatingChangedEventArgs : SelectedValueChangedEventArgs
    {
        public new FilterDataVm<float, float?> Source
        {
            get
            {
                return (FilterDataVm<float, float?>)base.Source;
            }
        }

        public new float? NewValue
        {
            get
            {
                return (float?)base.NewValue;
            }
        }

        public RatingChangedEventArgs(SelectedValueChangedEventArgs e) : base(e.Source, e.NewValue) { }
    }
}
