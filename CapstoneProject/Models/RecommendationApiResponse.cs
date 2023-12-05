using System;
namespace CapstoneProject.Models
{
	public class RecommendationApiResponse
	{
        public string UserId { get; set; }
        public List<RecommendationModel> Results { get; set; }
    }
}

