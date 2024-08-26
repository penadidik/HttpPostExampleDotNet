using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

class ContactController
{

    public static List<ContactEntity> GetListFromDB() {
        var items = new List<ContactEntity>();

        ContacRepository.Initialize();
        items = ContacRepository.getListContact();

        return items;
    }

}