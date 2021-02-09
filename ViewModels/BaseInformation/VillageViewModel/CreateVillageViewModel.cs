using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;

namespace ViewModels.BaseInformation.VillageViewModel
{
	public class CreateVillageViewModel
	{
		public Village Village { get; set; }


        [DisplayName("استان")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? StateId { get; set; }

        [DisplayName("شهر")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? CityId { get; set; }

        public List<Section> Sections { get; set; }
        public List<City> Cities { get; set; }
        public List<State> States { get; set; }
	}
}