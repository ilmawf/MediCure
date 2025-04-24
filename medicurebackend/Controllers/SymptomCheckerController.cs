using Microsoft.AspNetCore.Mvc;

namespace medicurebackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SymptomCheckerController : ControllerBase
    {
        // POST: api/symptom-checker
        [HttpPost]
        public ActionResult<string> PostSymptomChecker([FromBody] string symptoms)
        {
            // Basic logic to diagnose based on symptoms
            if (symptoms.Contains("fever") && symptoms.Contains("cough"))
            {
                return Ok(new { diagnosis = "You might have the flu." });
            }
            else if (symptoms.Contains("headache") && symptoms.Contains("nausea"))
            {
                return Ok(new { diagnosis = "You might have a migraine." });
            }
            else if (symptoms.Contains("chest pain") && symptoms.Contains("shortness of breath"))
            {
                return Ok(new { diagnosis = "You might have a heart condition. Please see a doctor immediately." });
            }

            return Ok(new { diagnosis = "Unable to diagnose. Please consult a doctor." });
        }
    }
}
