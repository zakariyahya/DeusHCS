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
        [Required]
        public string id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string EmployeeId { get; set; }

        [Required]
        public int Diplomatic { get; set; }

        [Required]
        public int Empathy { get; set; }

        [Required]
        public int Cooperative { get; set; }

        [Required]
        public int Trust { get; set; }

        [Required]
        public int Ambitious { get; set; }

        [Required]
        public int Competitive { get; set; }

        [Required]
        public int Direct { get; set; }

        [Required]
        public int Rational { get; set; }

        [Required]
        public int Conformity { get; set; }

        [Required]
        public int Diligent { get; set; }

        [Required]
        public int Organized { get; set; }

        [Required]
        public int Temperance { get; set; }

        [Required]
        public int Versatile { get; set; }

        [Required]
        public int Instinctive { get; set; }

        [Required]
        public int Opportunistic { get; set; }

        [Required]
        public int Unorthodox { get; set; }

        [Required]
        public int Adventurous { get; set; }

        [Required]
        public int Assertive { get; set; }

        [Required]
        public int Optimistic { get; set; }

        [Required]
        public int Gregarious { get; set; }

        [Required]
        public int Independent { get; set; }

        [Required]
        public int Introspective { get; set; }

        [Required]
        public int Restrained { get; set; }

        [Required]
        public int Ingenuity { get; set; }

        [Required]
        public int Inquisitive { get; set; }

        [Required]
        public int Intricate { get; set; }

        [Required]
        public int StrategicallyMinded { get; set; }

        [Required]
        public int Methodical { get; set; }

        [Required]
        public int PresentMinded { get; set; }

        [Required]
        public int Utilitarian { get; set; }

        [Required]
        public int Relationship_Driven { get; set; }

        [Required]
        public int ResultsDriven { get; set; }

        [Required]
        public int MotivatedByOrder { get; set; }

        [Required]
        public int MotivatedByAmbiguity { get; set; }

        [Required]
        public int Outgoing { get; set; }

        [Required]
        public int Reserved { get; set; }

        [Required]
        public int Innovative { get; set; }

        [Required]
        public int Pragmatic { get; set; }

        [Required]
        public int VisualPerception { get; set; }

        [Required]
        public int SpatialReasoning { get; set; }

        [Required]
        public int VerbalComprehension { get; set; }

        [Required]
        public int DominantLeadership { get; set; }

        [Required]
        public int CollaborativeLeadership { get; set; }

        [Required]
        public int ServantLeadership { get; set; }

        [Required]
        public int SupportingOthers { get; set; }

        [Required]
        public int CoachingOthers { get; set; }

        [Required]
        public int EnablingTeamwork { get; set; }

        [Required]
        public int BuildingConnections { get; set; }

        [Required]
        public int PersuadingOthers { get; set; }

        [Required]
        public int AdaptiveCommunication { get; set; }

        [Required]
        public int TrendIdentification { get; set; }

        [Required]
        public int DistillingInformation { get; set; }

        [Required]
        public int ApplyingExpertise { get; set; }

        [Required]
        public int ActiveLearning { get; set; }

        [Required]
        public int StrategicAgility { get; set; }

        [Required]
        public int ChampioningChange { get; set; }

        [Required]
        public int ForwardPlanning { get; set; }

        [Required]
        public int FollowingDirections { get; set; }

        [Required]
        public int QualityClientDelivery { get; set; }

        [Required]
        public int ResilienceUnderPressure { get; set; }

        [Required]
        public int LateralProblemSolving { get; set; }

        [Required]
        public int OperationalFlexibility { get; set; }

        [Required]
        public int SelfDevelopmentFocused { get; set; }

        [Required]
        public int BusinessAcumen { get; set; }

        [Required]
        public int OptimizingPerformance { get; set; }

        [Required]
        public int LeadingAndDeciding { get; set; }

        [Required]
        public int SupportingAndCooperating { get; set; }

        [Required]
        public int InteractingAndNegotiating { get; set; }

        [Required]
        public int AnalyzeAndInterpret { get; set; }

        [Required]
        public string CreateAndConceptualize { get; set; }

        [Required]
        public int OrganizeAndExecute { get; set; }

        [Required]
        public int AdaptAndCope { get; set; }

        [Required]
        public int EnterprisingAndPerforming { get; set; }

    }
}