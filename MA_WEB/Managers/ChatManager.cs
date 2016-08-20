using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Model.BusinessObjects;

namespace MA_WEB.Managers
{
    public class ChatManager:BaseManager
    {
        public ChatRecordBO Add(ChatRecordBO chatRecord)
        {
            if (chatRecord == null)
                throw new ArgumentNullException("chatRecord");
            if (chatRecord.Project == null)
                throw new ArgumentNullException("Project");
            if (chatRecord.User == null)
                throw new ArgumentNullException("User");
            if (String.IsNullOrEmpty(chatRecord.Message))
                throw new ArgumentNullException("Message");

            chatRecord = Context.ChatRecords.Add(chatRecord);
            Context.SaveChanges();

            return chatRecord;
        }

        public async Task<List<ChatRecordBO>> Get(ProjectBO project, int pageIndex, int pageSize)
        {
            if (project == null)
                throw new ArgumentNullException("project");
            if (project.Id <= 0)
                throw new ArgumentOutOfRangeException("project.Id");

            return await Context.ChatRecords.OrderByDescending(record => record.Id)
                .Where(record => record.Project.Id == project.Id)
                .Include(record => record.User)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}