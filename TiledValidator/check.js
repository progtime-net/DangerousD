let objectErrors = []
let layerErrors = []
let errCount
let cur = 0
let map

function getGamePath() {
    let file = new TextFile(`${tiled.project.extensionsPath}/.DDValidator`)
    let res = file.readLine()
    file.close()
    return res
}


var checkMistakes = tiled.registerAction("MistakesCheck", function(action) {
    objectErrors = []
    layerErrors = []
    cur = []
    map = tiled.activeAsset;
    if (!map.isTileMap) {
        tiled.alert("Not a tile map!");
        return;
    }

    // TODO: first object returns nothing, shiza!!!!!!!
    var prc = new Process()
    prc.start(getGamePath(), [])
    for (let i = map.layerCount - 1; i >= 0; i--) {

        const layer = map.layerAt(i);
        if (layer.isObjectLayer) {
            for (let i = layer.objectCount - 1; i >= 0; i--) {
                const obj = layer.objectAt(i)
                let cln = layer.className ? layer.className : ""
                if (obj.className){
                    if (cln) cln += `.${obj.className}`
                    else cln = obj.className
                }

                if (cln === "") {
                    objectErrors.push(obj)
                    continue
                }
                prc.writeLine(cln)
                prc.waitForFinished(100)
                let ans = prc.readLine()
                if (ans !== "OK") {
                    if (obj.className){
                        layerErrors.push(layer)
                        break
                    }
                    else objectErrors.push(obj)
                }
            }
        }
        else if (layer.isTileLayer) {
            if (layer.className === "") {
                layerErrors.push(layer)
                continue
            }
            prc.writeLine(layer.className)
            prc.waitForFinished(100)

            let ans = prc.readLine()
            if (ans !== "OK") layerErrors.push(layer)
        }
    }
    prc.writeLine("END")
    prc.close()
    errCount = layerErrors.length + objectErrors.length
    if (errCount > 0) {
        let resolve = tiled.confirm(`${layerErrors.length} layers and ${objectErrors.length} objects found whose classes do not exist in the game. Start resolving?`)

        if (resolve){
            next.enabled = true
            hc.enabled = true

            tiled.trigger("HighlightCur")
        }
    }
    else tiled.alert("No errors found!")
})
checkMistakes.text = "Check map for mistakes"
checkMistakes.shortcut = "Ctrl+Shift+C"

var hc = tiled.registerAction("HighlightCur", function(action) {
    if (cur < layerErrors.length){
        map.currentLayer = layerErrors[cur];
        tiled.log(`Resolving ${cur} error(layer id: ${layerErrors[cur].id})`)
    }
    else {
        let obj = objectErrors[cur - layerErrors.length]
        tiled.log(`Resolving ${cur} error(object id: ${obj.id})`)
        const pos = map.pixelToScreen ? map.pixelToScreen(obj.pos) :obj.pos;
        tiled.mapEditor.currentMapView.centerOn(pos.x, pos.y);
        map.selectedObjects = [obj];
    }
})
hc.text = "Highlight current"
hc.shortcut = "Ctrl+Up"
hc.enabled = false

var next = tiled.registerAction("NextErr", function(action) {
    cur++
    if (cur > errCount - 1) cur = 0
    if (cur > 0) prev.enabled = true
    tiled.trigger("HighlightCur")
})
next.text = "Next"
next.shortcut = "Ctrl+Right"
next.enabled = false


var prev = tiled.registerAction("PrevErr", function(action) {
    cur--
    if (!cur) prev.enabled = false
    if (cur < 0) cur = errCount - 1
    tiled.trigger("HighlightCur")
})
prev.text = "Prev"
prev.shortcut = "Ctrl+Left"
prev.enabled = false

tiled.extendMenu("Map", [
    { action: "MistakesCheck", before: "AutoMap" },
    { action: "PrevErr"},
    { action: "HighlightCur" },
    { action: "NextErr"},
    { separator: true }
]);
tiled.activeAssetChanged.connect(() => {
    next.enabled = false
    hc.enabled = false
    prev.enabled = false
})



