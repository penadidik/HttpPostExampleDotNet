class TemplateController
{

    public static String template_testing = "66949932db020a0e202048ae";
    public static List<TemplateEntity> SetTemplateTesting(List<ContactEntity> contacts) {
            var collectTemplates = new List<TemplateEntity>{};
            foreach (var item in contacts)
            {
                TemplateEntity temp = new TemplateEntity
                {
                    template_id = template_testing,
                    wa_id = item.Number,
                    components = []
                };
                collectTemplates.Add(temp);
            }
            return collectTemplates;
     }
}