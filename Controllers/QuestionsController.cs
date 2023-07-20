using FullstackQnA_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullstackQnA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly QuestionsContext _QuestionsContext;
        private readonly AnswersContext _AnswersContext;

        public QuestionsController(QuestionsContext questionsContext, AnswersContext answersContext)
        {
            _QuestionsContext = questionsContext;
            _AnswersContext = answersContext;
        }

        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
        {
            if (_QuestionsContext.Questions == null)
            {
                return NotFound();
            }
            return await _QuestionsContext.Questions.OrderByDescending(q => q.QuestionId).ToListAsync();
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int id)
        {
            if (_QuestionsContext.Questions == null)
            {
                return NotFound();
            }
            var question = await _QuestionsContext.Questions.FindAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            return question;
        }

        // PUT: api/Questions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(int id, Question question)
        {
            if (id != question.QuestionId)
            {
                return BadRequest();
            }

            _QuestionsContext.Entry(question).State = EntityState.Modified;

            try
            {
                await _QuestionsContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Questions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Question>> PostQuestion(Question question)
        {
            if (_QuestionsContext.Questions == null)
            {
                return Problem("Entity set 'QuestionsContext.Questions'  is null.");
            }

            // Set the time to south african time
            question.CreatedDate =
                TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
            // set the question as not answered
            question.QuestionAnswered = false;
            //Add the question to the context
            _QuestionsContext.Questions.Add(question); // we can assume adding the question to the context will create an id for it
            await _QuestionsContext.SaveChangesAsync();

            return CreatedAtAction("GetQuestion", new { id = question.QuestionId }, question);
        }

        /// <summary>
        /// Supports CORS
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {

            if (_QuestionsContext.Questions == null)
            {
                return NotFound();
            }
            var question = await _QuestionsContext.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            // We want to delete the answers to the question first since this answers are dependent on the question,
            // otherwise this dependence will cause an error
            var answer = await _AnswersContext.Answers.FirstOrDefaultAsync(ans => ans.QuestionId == id);

            if (answer != null)
            {
                _AnswersContext.Answers.Remove((Answer)answer);
                await _AnswersContext.SaveChangesAsync();
            }

            _QuestionsContext.Questions.Remove(question);
            await _QuestionsContext.SaveChangesAsync();

            return Ok(new { result = "Delete successful" });
            //return NoContent();
        }

        private bool QuestionExists(int id)
        {
            return (_QuestionsContext.Questions?.Any(e => e.QuestionId == id)).GetValueOrDefault();
        }
    }
}
