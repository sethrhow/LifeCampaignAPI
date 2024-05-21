using Microsoft.AspNetCore.Mvc;
using LifeCampaignAPI.Models;
using LifeCampaignAPI.Data;
using LiteDB;
using System.Linq;

namespace LifeCampaignAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ObjectiveController(LiteDbContext db) : ControllerBase
    {
        #region Campaign CRUD

        [HttpGet]
        [Route("/campaign/")]
        public IActionResult GetAllCampaigns()
        {
            return Ok(db.Campaign.FindAll());
        }

        [HttpGet]
        [Route("/campaign/{id}")]
        public IActionResult GetCampaign(Guid id)
        {
            var campaign = db.Campaign.FindById(id);
            if (campaign != null)
                return Ok(campaign);

            return NotFound();
        }

        [HttpPost]
        [Route("/campaign/")]
        public IActionResult PostCampaign(Campaign campaign)
        {
            if (db.Campaign.Upsert(campaign))
                return Ok(db.Campaign.FindById(campaign.Id));

            return NotFound();
        }

        [HttpDelete]
        [Route("/campaign/{id}")]
        public IActionResult DeleteCampaign(Guid id)
        {
            if (db.Campaign.Delete(id))
                return Ok();

            return NotFound();
        }

        #endregion
        #region Mission CRUD

        [HttpGet]
        [Route("/mission/{id}")]
        public IActionResult GetMissionById(Guid id)
        {
            var mission = db.Mission.FindById(id);
            if (mission != null)
                return Ok(mission);

            return NotFound();
        }

        [HttpGet]
        [Route("/campaign/{id}/mission")]
        public IActionResult GetMissionsByCampaignId(Guid id)
        {
            var campaign = db.Campaign.FindById(id);
            if (campaign != null)
                return Ok(db.Mission.FindAll().Where(m => campaign.MissionIds.Contains(m.Id)));

            return NotFound();
        }

        [HttpPost]
        [Route("/mission/")]
        public IActionResult PostMission(Mission mission)
        {
            if (db.Mission.Upsert(mission))
                return Ok(db.Mission.FindById(mission.Id));

            return NotFound();
        }

        [HttpDelete]
        [Route("/mission/")]
        public IActionResult DeleteMission(Guid id)
        {
            if (db.Mission.Delete(id))
                return Ok();

            return NotFound();
        }

        #endregion
        #region Objective CRUD

        [HttpGet]
        [Route("/objective/{id}")]
        public IActionResult GetObjectiveById(Guid id)
        {
            var objective = db.Objective.FindById(id);
            if (objective != null)
                return Ok(objective);

            return NotFound();
        }

        [HttpGet]
        [Route("/objective/")]
        public IActionResult GetAllObjectives()
        {
            var objectives = db.Objective.FindAll();
            if (objectives != null)
                return Ok(objectives);

            return NotFound();
        }

        [HttpGet]
        [Route("/mission/{id}/objective")]
        public IActionResult GetObjectivesByMissionId(Guid id)
        {
            var mission = db.Mission.FindById(id);
            if (mission != null)
                return Ok(db.Objective.FindAll().Where(o => mission.ObjectiveIds.Contains(o.Id)));

            return NotFound();
        }

        [HttpPost]
        [Route("/objective/")]
        public IActionResult PostObjective(Objective objective)
        {
            if (db.Objective.Upsert(objective))
                return Ok(db.Objective.FindById(objective.Id));

            return NotFound();
        }

        [HttpDelete]
        [Route("/objective/{id}")]
        public IActionResult DeleteObjective(Guid id)
        {
            if (db.Objective.Delete(id))
                return Ok();

            return NotFound();
        }

        #endregion
        #region Player CRUD

        [HttpGet]
        [Route("/player/{id}")]
        public IActionResult GetPlayerById(Guid id)
        {
            var player = db.Player.FindById(id);
            if (player != null)
                return Ok(player);

            return NotFound();
        }

        [HttpGet]
        [Route("/player/")]
        public IActionResult GetPlayers()
        {
            var players = db.Player.FindAll();
            if (players != null)
                return Ok(players);

            return NotFound();
        }
        #endregion
    }
}
