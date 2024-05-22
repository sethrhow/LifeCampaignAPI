using Microsoft.AspNetCore.Mvc;
using LifeCampaignAPI.Models;
using LifeCampaignAPI.Data;
using LiteDB;
using System.Linq;
using System.Reflection;

namespace LifeCampaignAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ObjectiveController(LiteDbContext db) : ControllerBase
    {

        // Clear database & generate sample campaign
        [HttpPost]
        [Route("/database/reset")]
        public IActionResult SeedDb()
        {
            db.Campaign.DeleteAll();
            db.Mission.DeleteAll();
            db.Objective.DeleteAll();
            db.PlayerHistory.DeleteAll();
            db.Player.DeleteAll();

            var objectives = new List<Objective>
            {
                // Determine Location
                new() {
                    Title = "Make a List",
                    Description = "Prepare a list of locations to choose from.",
                    Detail = "List out as many places as you can. If it's open to the public and allows more than one person in at a time, it counts." },
                new() {
                    Title = "Sort the List",
                    Description = "Use a scoring metric to help determine the best location",
                    Detail = "For each location, if you would feel comfortable going there, add a point. " +
                             "If there would be several people (not employees) to talk to, add two points. " +
                             "If the location is a traditionally social place (dog park, bar, etc.) add three points." +
                             "Select the highest scoring location and prepare yourself."
                    },
                new()
                {
                    Title = "Prepare yourself",
                    Description = "Gird your loins and go to your selected location.",
                    Detail = "Take a shower, apply deoderant, and dress yourself appropriately. " +
                             "Wear the outfit you will be least self-concious in. " +
                             "Don't overthink it! The goal here is to dress like a normal human being, " +
                             "which will most likley be jeans and a t-shirt."
                },

                // Select Someone
                new()
                {
                    Title = "People Watching",
                    Description = "Take a minute to relax and observe your surroundings.",
                    Detail = "Get something to eat or drink (if applicable). " +
                             "Find somewhere comfortable where you can relax for a moment and observe the people around you. " +
                             "Select a person who doesn't appear to be in a hurry to leave."
                },
                new()
                {
                    Title = "Reason to Talk",
                    Description = "It's easier to start a conversation with a question.",
                    Detail = "This is the second hardest objective in the mission. Start small! " +
                             "Ask for the time. Ask if it's going to rain. Ask for a coffee/beer recommendation (if applicable). Ask a silly hypothetical." +
                             "The goal is to increase your confidence through repeat exposure. " +
                             "You can repeat this mission as many times as you need. "
                },

                // Initiate Conversation
                new()
                {
                    Title = "Approach",
                    Description = "Approach the person you selected and ask your opening question.",
                    Detail = "Gird your loins, put your phone away, and approach the person you selected. " +
                             "If you're feeling nervous, it may help to imagine that you work here " +
                             "and the single glorious purpose of your job is to get an answer to your question. "
                },
                new()
                {
                    Title = "Initiate",
                    Description = "Say \"Excuse me,\" and ask your question.",
                    Detail = "This is the moment you have prepared for. You have chosen the location. " +
                    "You have chosen your outfit. You have chosen the person to talk to. You have approached this person. " +
                    "You are in control. You are the master of your own destiny. Ask your question, and receive your answer. " +
                    "And then you can say \"Thanks!\" and leave, or stay and ask more questions if you're comfortable. " +
                    "As you repeat this mission, you will gain confidence and learn valuable social skills. " +
                    "Eventually, you will meet someone you could be friends with. And when that happens, don't forget to ask " +
                    "for their phone number."
                }
            };
            var missions = new List<Mission>
            {
                new() {
                    Title = "Determine Location",
                    Description = "Find the best place to meet someone in your area.",
                    ObjectiveIds = { objectives[0].Id, objectives[1].Id, objectives[2].Id }
                },
                new()
                {
                    Title = "Select Someone", 
                    Description = "Observe the people around you and select the right person to talk to.",
                    ObjectiveIds = { objectives[3].Id, objectives[4].Id }
                },
                new() 
                { 
                    Title = "Initiate Conversation", 
                    Description = "Make your approach and initiate the conversation.",
                    ObjectiveIds = { objectives[5].Id, objectives[6].Id }
                }
            };
            var campaign = new Campaign()
            {
                Title = "Find a friend",
                Description = "Learn how to initiate a conversation with a stranger in a public place. " +
                              "Gradually gain confidence and social skills, and eventually make a friend!",
                MissionIds = { missions[0].Id, missions[1].Id, missions[2].Id }
            };

            db.Objective.InsertBulk(objectives);
            db.Mission.InsertBulk(missions);
            db.Campaign.Insert(campaign);

            return Ok();
        }

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

        [HttpPost]
        [Route("/player/")]
        public IActionResult PostPlayer(Player player)
        {
            if (db.Player.Upsert(player))
                return Ok(db.Player.FindById(player.Id));

            return NotFound();
        }

        [HttpDelete]
        [Route("/player/{id}")]
        public IActionResult DeletePlayer(Guid id)
        {
            if (db.Player.Delete(id))
                return Ok();

            return NotFound();
        }

        #endregion
        #region PlayerHistory CRUD

        [HttpGet]
        [Route("/playerhistory/{id}")]
        public IActionResult GetPlayerHistoryById(Guid id)
        {
            var playerHistory = db.PlayerHistory.FindById(id);
            if (playerHistory != null)
                return Ok(playerHistory);

            return NotFound();
        }

        [HttpGet]
        [Route("/player/{id}/playerhistory/")]
        public IActionResult GetPlayerHistoriesByPlayerId(Guid id)
        {
            var playerHistories = db.PlayerHistory.FindAll().Where(p => p.PlayerId == id);
            if (playerHistories != null)
                return Ok(playerHistories);

            return NotFound();
        }

        [HttpGet]
        [Route("/playerhistory/")]
        public IActionResult GetPlayerHistories()
        {
            var playerHistories = db.PlayerHistory.FindAll();
            if (playerHistories != null)
                return Ok(playerHistories);

            return NotFound();
        }

        [HttpPost]
        [Route("/playerhistory/")]
        public IActionResult PostPlayerHistory(PlayerHistory playerHistory)
        {
            if (db.PlayerHistory.Upsert(playerHistory))
                return Ok(db.PlayerHistory.FindById(playerHistory.Id));

            return NotFound();
        }

        [HttpDelete]
        [Route("/playerhistory/{id}")]
        public IActionResult DeletePlayerHistory(Guid id)
        {
            if (db.PlayerHistory.Delete(id))
                return Ok();

            return NotFound();
        }

        #endregion
    }
}
