#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Services.Contracts
{
    public interface INetworkService
    {

        Task<long> Invite(string email, string inviter = null);
#pragma warning disable CS0246 // The type or namespace name 'ResponseContext' could not be found (are you missing a using directive or an assembly reference?)
        Task<ResponseContext> InviteByPhone(string country, string mobile);
#pragma warning restore CS0246 // The type or namespace name 'ResponseContext' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'ConnectionEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<ConnectionEntity> Connect(string email, string inviter);
#pragma warning restore CS0246 // The type or namespace name 'ConnectionEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<int> Disconnect(long id, string username);
#pragma warning disable CS0246 // The type or namespace name 'ConnectionEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'ConnectionFilter' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<ConnectionEntity>> Connections(ConnectionFilter filter);
#pragma warning restore CS0246 // The type or namespace name 'ConnectionFilter' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'ConnectionEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'ConnectionEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<ConnectionEntity> Get(long id);
#pragma warning restore CS0246 // The type or namespace name 'ConnectionEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'ConnectionEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<ConnectionEntity> Get(long userId, string email);
#pragma warning restore CS0246 // The type or namespace name 'ConnectionEntity' could not be found (are you missing a using directive or an assembly reference?)

#pragma warning disable CS0246 // The type or namespace name 'ContactByCountry' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<ContactByCountry>> ContactList(long? countryId, DateTime? start, DateTime? end);
#pragma warning restore CS0246 // The type or namespace name 'ContactByCountry' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'UserContactsByCountry' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<UserContactsByCountry>> UserContactList(long countryId, string name = null, DateTime? start=null, DateTime? end=null, int? offset = null, int? size = null);
#pragma warning restore CS0246 // The type or namespace name 'UserContactsByCountry' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'UserContact' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<UserContact>> ContactListByUser(long userId, string name = null, DateTime? start = null, DateTime? end = null, int? offset = null, int? size = null);
#pragma warning restore CS0246 // The type or namespace name 'UserContact' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'SMSStatusEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<SMSStatusEntity>> ViewStatus(long Id);
#pragma warning restore CS0246 // The type or namespace name 'SMSStatusEntity' could not be found (are you missing a using directive or an assembly reference?)
        Task<int> DeleteContact(long Id);        
    }
}
