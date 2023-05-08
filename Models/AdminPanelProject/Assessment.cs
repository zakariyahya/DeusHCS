using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAdmin.Models.adminPanelProject
{
    [Table("Assessments", Schema = "public")]
    public partial class Assessment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }

        [Required]
        public string UserId { get; set; }
    
        public string EmployeeId { get; set; }

        [Required]
        public decimal Diplomatic { get; set; }

        [Required]
        public decimal Empathy { get; set; }

        [Required]
        public decimal Cooperative { get; set; }

        [Required]
        public decimal Trust { get; set; }

        [Required]
        public decimal Ambitious { get; set; }

        [Required]
        public decimal Competitive { get; set; }

        [Required]
        public decimal Direct { get; set; }

        [Required]
        public decimal Rational { get; set; }

        [Required]
        public decimal Conformity { get; set; }

        [Required]
        public decimal Diligent { get; set; }

        [Required]
        public decimal Organized { get; set; }

        [Required]
        public decimal Temperance { get; set; }

        [Required]
        public decimal Instinctive { get; set; }

        [Required]
        public decimal Versatile { get; set; }

        [Required]
        public decimal Opportunistic { get; set; }

        [Required]
        public decimal InteractingAndNegotiating { get; set; }

        [Required]
        public decimal AnalyzeAndInterpret { get; set; }

        [Required]
        public decimal Unorthodox { get; set; }
        [Required]
        public decimal Introspective { get; set; }

        [Required]
        public decimal Intricate { get; set; }

        [Required]
        public decimal Adventurous { get; set; }

        [Required]
        public decimal Assertive { get; set; }

        [Required]
        public decimal Optimistic { get; set; }

        [Required]
        public decimal Gregarious { get; set; }

        [Required]
        public decimal Independent { get; set; }

        [Required]
        public decimal decimalrospective { get; set; }

        [Required]
        public decimal Restrained { get; set; }

        [Required]
        public decimal Ingenuity { get; set; }

        [Required]
        public decimal Inquisitive { get; set; }

        [Required]
        public decimal decimalricate { get; set; }

        [Required]
        public decimal StrategicallyMinded { get; set; }

        [Required]
        public decimal Methodical { get; set; }

        [Required]
        public decimal PresentMinded { get; set; }

        [Required]
        public decimal Utilitarian { get; set; }

        [Required]
        public decimal Relationship_Driven { get; set; }

        [Required]
        public decimal ResultsDriven { get; set; }

        [Required]
        public decimal MotivatedByOrder { get; set; }

        [Required]
        public decimal MotivatedByAmbiguity { get; set; }

        [Required]
        public decimal Outgoing { get; set; }

        [Required]
        public decimal Reserved { get; set; }

        [Required]
        public decimal Innovative { get; set; }

        [Required]
        public decimal Pragmatic { get; set; }

        [Required]
        public decimal VisualPerception { get; set; }

        [Required]
        public decimal SpatialReasoning { get; set; }

        [Required]
        public decimal VerbalComprehension { get; set; }

        [Required]
        public decimal DominantLeadership { get; set; }

        [Required]
        public decimal CollaborativeLeadership { get; set; }

        [Required]
        public decimal ServantLeadership { get; set; }

        [Required]
        public decimal SupportingOthers { get; set; }

        [Required]
        public decimal CoachingOthers { get; set; }

        [Required]
        public decimal EnablingTeamwork { get; set; }

        [Required]
        public decimal BuildingConnections { get; set; }

        [Required]
        public decimal PersuadingOthers { get; set; }

        [Required]
        public decimal AdaptiveCommunication { get; set; }

        [Required]
        public decimal TrendIdentification { get; set; }

        [Required]
        public decimal DistillingInformation { get; set; }

        [Required]
        public decimal ApplyingExpertise { get; set; }

        [Required]
        public decimal ActiveLearning { get; set; }

        [Required]
        public decimal StrategicAgility { get; set; }

        [Required]
        public decimal ChampioningChange { get; set; }

        [Required]
        public decimal ForwardPlanning { get; set; }

        [Required]
        public decimal FollowingDirections { get; set; }

        [Required]
        public decimal QualityClientDelivery { get; set; }

        [Required]
        public decimal ResilienceUnderPressure { get; set; }

        [Required]
        public decimal LateralProblemSolving { get; set; }

        [Required]
        public decimal OperationalFlexibility { get; set; }

        [Required]
        public decimal SelfDevelopmentFocused { get; set; }

        [Required]
        public decimal BusinessAcumen { get; set; }

        [Required]
        public decimal OptimizingPerformance { get; set; }

        [Required]
        public decimal LeadingAndDeciding { get; set; }

        [Required]
        public decimal SupportingAndCooperating { get; set; }

        [Required]
        public decimal decimaleractingAndNegotiating { get; set; }

        [Required]
        public decimal AnalyzeAnddecimalerpret { get; set; }

        [Required]
        public decimal CreateAndConceptualize { get; set; }

        [Required]
        public decimal OrganizeAndExecute { get; set; }

        [Required]
        public decimal AdaptAndCope { get; set; }

        [Required]
        public decimal EnterprisingAndPerforming { get; set; }
    }
}
