#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Services
{
    public class NetworkService : DataService, INetworkService
    {
        //IUserService iUserService;
        public NetworkService()
        {
            //this.iUserService = iUserService;
        }
#pragma warning disable CS0246 // The type or namespace name 'ConnectionEntity' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<ConnectionEntity> Connect(string email, string inviter)
#pragma warning restore CS0246 // The type or namespace name 'ConnectionEntity' could not be found (are you missing a using directive or an assembly reference?)
        {                       
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("Email", email),
                new Parameter("Inviter", inviter)
            };

            return await SingleAsync<ConnectionEntity>("Connect", parameters);            
        }


        public async Task<int> Disconnect(long id, string username)
        {            
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("Id", id),
                new Parameter("Username", username)
            };
            return await ScalerAsync<int>("Disconnect", parameters);            
        }

        public async Task<long> Invite(string email, string inviter = null)
        {
            long id = 0;
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("Email", email),
                new Parameter("Inviter", inviter)
            };

            if (!string.IsNullOrEmpty(email))
            {            
                id = await ScalerAsync<long>("InviteViaEmail", parameters);
            }
            return id;
        }

#pragma warning disable CS0246 // The type or namespace name 'ResponseContext' could not be found (are you missing a using directive or an assembly reference?)
        public Task<ResponseContext> InviteByPhone(string country, string mobile)
#pragma warning restore CS0246 // The type or namespace name 'ResponseContext' could not be found (are you missing a using directive or an assembly reference?)
        {
            throw new NotImplementedException();
        }

#pragma warning disable CS0246 // The type or namespace name 'ConnectionEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'ConnectionFilter' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<ConnectionEntity>> Connections(ConnectionFilter filter)
#pragma warning restore CS0246 // The type or namespace name 'ConnectionFilter' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'ConnectionEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("UserId", filter.UserId),
                new Parameter("Name", filter.Name),
                new Parameter("Status", filter.Status),
                new Parameter("PageNumber", filter.PageNumber),
                new Parameter("PageSize", filter.PageSize),
            };

            return await ReadAsync<ConnectionEntity>("ConnectionList", parameters);
        }


#pragma warning disable CS0246 // The type or namespace name 'ConnectionEntity' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<ConnectionEntity> Get(long id)
#pragma warning restore CS0246 // The type or namespace name 'ConnectionEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("Id", id)
            };

            return await SingleAsync<ConnectionEntity>("ConnectionSingle", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'ConnectionEntity' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<ConnectionEntity> Get(long userId, string email)
#pragma warning restore CS0246 // The type or namespace name 'ConnectionEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("UserId", userId),
                new Parameter("Email", email),
            };

            return await SingleAsync<ConnectionEntity>("ConnectionSingle", parameters);
        }


#pragma warning disable CS0246 // The type or namespace name 'ContactByCountry' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<ContactByCountry>> ContactList(long? countryId, DateTime? start, DateTime? end)
#pragma warning restore CS0246 // The type or namespace name 'ContactByCountry' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("CountryId", countryId),
                new Parameter("Start", start),
                new Parameter("End", end)                
            };

            return await ReadAsync<ContactByCountry>("ContactsByCountry", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'UserContactsByCountry' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<UserContactsByCountry>> UserContactList(long countryId, string name = null, DateTime? start = null, DateTime? end = null, int? offset = null, int? size = null)
#pragma warning restore CS0246 // The type or namespace name 'UserContactsByCountry' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {                
                new Parameter("CountryId", countryId),
                new Parameter("Name", name),
                new Parameter("Start", start),
                new Parameter("End", end),
                new Parameter("Offset", offset),
                new Parameter("Size", size)                
            };

            return await ReadAsync<UserContactsByCountry>("UserContactsByCountry", parameters);
        }
#pragma warning disable CS0246 // The type or namespace name 'UserContact' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<UserContact>> ContactListByUser(long userId, string name = null, DateTime? start = null, DateTime? end = null, int? offset = null, int? size = null)
#pragma warning restore CS0246 // The type or namespace name 'UserContact' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {                
                new Parameter("UserId", userId),
                new Parameter("Name", name),
                new Parameter("Start", start),
                new Parameter("End", end),
                new Parameter("Offset", offset),
                new Parameter("Size", size)                
            };

            return await ReadAsync<UserContact>("ContactListByUser", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'SMSStatusEntity' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<SMSStatusEntity>> ViewStatus(long Id)
#pragma warning restore CS0246 // The type or namespace name 'SMSStatusEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>()
            {                
                new Parameter("ContactId", Id),                
            };

            return await ReadAsync<SMSStatusEntity>("ViewStatusList", parameters);
        }

        public async Task<int> DeleteContact(long Id)
        {
            List<Parameter> parameters = new List<Parameter>()
            {                
                new Parameter("Id", Id),                
            };

            return await HandleDataAsync("PhoneContactDelete", parameters);
        }       
    }
}
