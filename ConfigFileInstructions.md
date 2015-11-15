Configuration file example:
```
<?xml version="1.0" encoding="utf-8" ?>
<Site>
  <Name>Site name</Name>
  <RootUrl>http://amazon.com/</RootUrl>

  <CrawlUrlsConfig>

    <MaxDepth>3</MaxDepth>

    <StartPoint>http://amazon.com/</StartPoint>    

    <NoFollow>
      <RegEx>nf regex 1</RegEx>
      <RegEx>nf regex 2</RegEx>
    </NoFollow>

  </CrawlUrlsConfig>


  <PageExtractionElements>
    <Element Name="Head tag" RegEx="&lt;head&gt;([.\s\S]*?)&lt;/head&gt;">
      <Field Name="Title tag" RegEx="&lt;(title[.\s\S]*?)&lt;/title&gt;" />      
    </Element>
  </PageExtractionElements>

</Site>
```


Fields: ( **^** - Element can be duplicated )

Name - The site name.

RootUrl - The root Uri address (http://example.com).

MaxDepth - Max links depth of crawling.

^StartPoint - If you don't want to start crawling from the RootUrl define the page to start from.

NoFollow - The Uris matching the ^RegEx elements inside will not be crawled.

PageExtractionElements - The container for the elements to capture.

^Element - The HTML section that defines the element. Name = <Name of captured element>, RegEx = <The Regular expression to look for>.

^Field - A field with in the parent element to capture. Name = <Field name>, RegEx = <The Regular expression to look for with-in the parent element text>.
fields will be saved as part of their parent element.

**Important:** All RegEx captures has to be defined as a group (in brackets).