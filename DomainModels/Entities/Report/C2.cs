using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModels.Entities.Report
{
   public  class C2
    {
        [DisplayName("ملی")]
       public Int32? tenderState { get; set; }
        [DisplayName("استانی")]
        public Int32? tendercountry { get; set; }
        public Int32? StateState { get; set; }
        public Int32? Statecountry { get; set; }
        public Int32? RecoupmentState { get; set; }
        public Int32? Recoupmentcountry { get; set; }
        public Int32? OpenDatesState { get; set; }
        public Int32? OpenDatescountry { get; set; }
        public Int32? EvaluationState { get; set; }
        public Int32? Evaluationcountry { get; set; }
        public Int32? TechnicalState { get; set; }
        public Int32? Technicalcountry { get; set; }
        public Int32? TechnicalEconomyState { get; set; }
        public Int32? TechnicalEconomycountry { get; set; }
        public Int32? ConcedeState { get; set; }
        public Int32? Concedecountry { get; set; }
        public Int32? OperationTitleState { get; set; }
        public Int32? OperationTitlecountry { get; set; }
        public Int32? ParticipantState { get; set; }
        public Int32? Participantcountry { get; set; }
        public Int32? ComplementarityDateState { get; set; }
        public Int32? ComplementarityDatecountry { get; set; }
        public Int32? ContractState { get; set; }
        public Int32? Contractcountry { get; set; }
    }
}
