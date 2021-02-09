using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.BaseInformation.SectionViewModel
{
    public class CreateSectionViewModel
	{
        public Section Section { get; set; }

        [DisplayName("استان")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? StateId { get; set; }
        
        public List<City> Cities { get; set; }
        public List<State> States { get; set; }
	}
}