using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;

namespace ViewModels.BaseInformation.RuralDistrictViewModel
{
	public class CreateRuralDistrictViewModel
	{
		public RuralDistrict RuralDistrict { get; set; }


        [DisplayName("استان")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? StateId { get; set; }

        [DisplayName("شهر")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? CityId { get; set; }

        [DisplayName("بخش")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? SectionId { get; set; }

        public List<Village> Villages { get; set; }
        public List<Section> Sections { get; set; }
        public List<City> Cities { get; set; }
        public List<State> States { get; set; }
	}
}