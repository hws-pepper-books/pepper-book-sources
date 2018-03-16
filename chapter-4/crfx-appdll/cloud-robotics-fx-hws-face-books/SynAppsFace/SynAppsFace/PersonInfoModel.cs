using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using HwsFaceCores;

namespace SynAppsFace
{
    class PersonInfoModel
    {
        private static SqlConnectionStringBuilder SqlBuilder = new SqlConnectionStringBuilder();

        public long Id { set; get; }
        public string PersonGroupId { set; get; }
        public string PersonId { set; get; }
        public string PersonName { set; get; }
        public string PersonNameYomi { set; get; }
        public DateTimeOffset? LastDetectedAt { set; get; }
        public bool IsDeleted { set; get; }
        public DateTimeOffset? CreatedAt { set; get; }
        public DateTimeOffset? UpdatedAt { set; get; }

        PersonInfoModel()
        {
            this.Id = 0;
            this.IsDeleted = false;
        }

        public static void Connection(string _connectionString)
        {
            SqlBuilder.ConnectionString = _connectionString;
        }

        public static PersonInfoModel New()
        {
            return new PersonInfoModel();
        }

        public static PersonInfoModel Find(string _personId)
        {
            var dc = new PersonInfoDataContext(SqlBuilder.ConnectionString);
            var records =
                from n in dc.PersonInfos
                where n.PersonId == _personId
                where n.IsDeleted == false
                select n;

            return Build(records);
        }

        public void Save()
        {
            var now = DateTime.UtcNow;
            var dc = new PersonInfoDataContext(SqlBuilder.ConnectionString);
            if (this.Id == 0)
            {
                dc.PersonInfos.InsertOnSubmit(new PersonInfo
                {
                    PersonGroupId  = this.PersonGroupId,
                    PersonId       = this.PersonId,
                    PersonName     = this.PersonName,
                    PersonNameYomi = this.PersonNameYomi,
                    LastDetectedAt = now,
                    IsDeleted      = this.IsDeleted,
                    CreatedAt      = now,
                    UpdatedAt      = now
                });
            }
            else
            {
                var records =
                    from n in dc.PersonInfos
                    where n.Id == this.Id
                    select n;

                foreach (var r in records)
                {
                    r.PersonName     = this.PersonName;
                    r.PersonNameYomi = this.PersonNameYomi;
                    r.LastDetectedAt = (this.LastDetectedAt.HasValue ? this.LastDetectedAt.Value.LocalDateTime : r.LastDetectedAt);
                    r.IsDeleted      = this.IsDeleted;
                    r.UpdatedAt      = now;
                }
            }
            dc.SubmitChanges();
        }

        public void Delete()
        {
            if (this.Id == 0) { return; }

            this.IsDeleted = true;
            Save();
        }

        private static PersonInfoModel Build(IQueryable<PersonInfo> records)
        {
            if (records.Count() == 0) { return PersonInfoModel.New(); }

            var model            = PersonInfoModel.New();
            model.Id             = records.First<PersonInfo>().Id;
            model.PersonGroupId  = records.First<PersonInfo>().PersonGroupId;
            model.PersonId       = records.First<PersonInfo>().PersonId;
            model.PersonName     = records.First<PersonInfo>().PersonName;
            model.PersonNameYomi = records.First<PersonInfo>().PersonNameYomi;
            model.IsDeleted      = (bool)(records.First<PersonInfo>().IsDeleted.HasValue ? records.First<PersonInfo>().IsDeleted : false);
            model.LastDetectedAt = records.First<PersonInfo>().LastDetectedAt;
            model.CreatedAt      = records.First<PersonInfo>().CreatedAt;
            model.UpdatedAt      = records.First<PersonInfo>().UpdatedAt;

            return model;
        }
    }
}
