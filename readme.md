# LuminaAPI Demo
A quick demo of how you can make your very own API with Lumina. This has some issues but are largely fixable with minimal effort, all in less than 200 lines of code!!!

Some quick things:
* Requires the `ffxiv/0a*` files to be located in the datapath you provide
* Some things break, due to cyclic references
* The initial load of a sheet will be _slowish_ but subsequent loads will be very fast (< 1ms) as the data will be cached in memory from thereon
* Lumina is single threaded, normally this isn't a problem but you will need to warm up the row cache for each sheet if you want to avoid this problem when the api is under high load


# Usage
Hit `<api host>/sheets/{language}/{sheetName}/{rowId}[/{subRowId}}` and you get data back.

`subRowId` is optional and not required, only provide if you require it

Language can be any of the following:
* `None` or 0
* `Japanese` or 1
* `English` or 2
* `German` or 3
* `French` or 4
* `ChineseSimplified` or 5
* `ChineseTraditional` or 6
* `Korean` or 7

Note that korean and chinese support requires game files that come with those languages!
