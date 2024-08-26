using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

class ContactController
{

    public static List<ContactEntity> GetListDummy() {
        

        var items = new List<ContactEntity>();

        for (int i = 5; i < 10; i++)
        {
            var data = new ContactEntity();
            data.Name = "Didik";
            data.Number = "628781253810" + i;
            items.Add(data);
        }   

        return items;
    }

    public static List<ContactEntity> GetListJson() {
        var items = new List<ContactEntity>();

        ContacRepository.Setup();
        items = ContacRepository.getListContact();

        return items;
    }

}