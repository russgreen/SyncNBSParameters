# SyncNBSParameters

NBS Chorus now syncs additional parameter values from NBC Chorus to Revit.  These parameters are listed here: https://support.thenbs.com/support/solutions/articles/7000043835

The following parameters are duplicates of data already contained in IFC parameters so the sync process will not overwrite/update the IFC parameters. This is not desirable as the data will be incorrect in tags and schedules that correctly reference other parameters on objects.

| Component object | Material object | Corresponding Chorus specification field |
| :--- | :---: | :--- |
| NBSChorusManName* [911E28F9-12DD-40A8-B0D5-014F9522F86A]| NBSChorusManName_mtrl* [69574448-B9B5-45D7-BC5F-38B193B320D7]| The article or clause 'Manufacturer', 'Supplied by' or 'Supplier'. |
| NBSChorusProdRef* [BE6F6DF3-763C-405A-9753-70306FF673D4]| NBSChorusProdRef_mtrl* [B1E5D0F4-44D8-4084-AFC7-4AE940E59B66]| The article or clause 'Product reference'. |
| NBSChorusManProdURL* [E12E541C-B092-4439-B9A3-9F7D070BC4C3]| NBSChorusManProdURL_mtrl* [8E182EF2-EBA8-43DF-8387-86106BE35563]| The URL of the supplier's product listing in NBS Source or Archify. |

The purpose of this addin is to allow users to sync values from these parameters into any other parameter in the project.  This is done by creating a mapping between the NBS parameter and the target parameter.  The mapping is stored in the project and can be edited anytime. Default settings are:

| NBS Chorus Parameter | Target Sync Parameter |
| :--- | :--- |
| NBSChorusManName | ManufacturerName [8cbb36e4-83db-4c85-b24e-6d0f0362f6af] |
| NBSChorusProdRef | ModelReference [3abb6113-6a66-4be1-bf5e-fc7ab310530f] |
| NBSChorusManProdURL | ProductInformation [133c86a9-e00e-4550-937c-7220daed735b] |
| NBSChorusManName_mtrl | ManufacturerName_mtrl [715dedbc-b126-4ecd-9ac8-c438782dd8c2] |
| NBSChorusProdRef_mtrl | ModelReference_mtrl [79b9aaae-be62-4459-ac43-ca48a7a36c18] |
| NBSChorusManProdURL_mtrl | ProductInformation_mtrl [a18cb312-db3c-4f3c-a90a-53e2d443c15b] |
