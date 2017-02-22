namespace Sitecore.Support.ExperienceEditor.Speak.Ribbon.Requests.SaveItem
{
    using Sitecore;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.ExperienceEditor.Speak.Ribbon.Requests.SaveItem;
    using Sitecore.ExperienceEditor.Speak.Server.Responses;
    using Sitecore.ExperienceEditor.Switchers;
    using Sitecore.Links;
    using Sitecore.Pipelines;
    using Sitecore.Pipelines.Save;
    using System;
    using System.Collections;
     
    public class CallServerSavePipelinePatch : CallServerSavePipeline
    { 
        public override PipelineProcessorResponseValue ProcessRequest()
        {
            PipelineProcessorResponseValue value2;
            PipelineProcessorResponseValue value3 = new PipelineProcessorResponseValue();
            Pipeline pipeline = PipelineFactory.GetPipeline("saveUI");
            pipeline.ID = ShortID.Encode(ID.NewID);
            SaveArgs saveArgs = base.RequestContext.GetSaveArgs();
            using (new ClientDatabaseSwitcher(base.RequestContext.Item.Database))
            {
                pipeline.Start(saveArgs);
                value3.AbortMessage = saveArgs.Error;
                value2 = value3;
            }
            PipelineProcessorResponseValue value4 = value2;
            ArrayList savedItems = saveArgs.SavedItems;
            if ((savedItems != null) && (savedItems.Count > 0))
            {
                Item item = savedItems[0] as Item;
                foreach (Item tempItem in savedItems)
                {
                    if (tempItem.ID == base.RequestContext.Item.ID)
                        item = tempItem;
                }

                if (item != null)
                {
                    string itemUrl = LinkManager.GetItemUrl(Context.Database.Items[item.ID, item.Language, item.Version]);
                    itemUrl = string.Format(itemUrl + "?sc_mode=edit&sc_itemid={0}&sc_lang={1}&sc_version={2}&sc_site={3}", item.ID, item.Language, item.Version.Number, Sitecore.Context.Site.Name);
                    value4.Value = new { url = itemUrl };
                }
            } 
            return value4;
        }
    }
}


