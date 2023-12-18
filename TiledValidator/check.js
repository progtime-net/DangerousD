let objectErrors = []
let layerErrors = []


function getGamePath() {
    let file = new TextFile(`${tiled.project.extensionsPath}/.DDValidator`)
    return file.readLine()
}


var checkMistakes = tiled.registerAction("MistakesCheck", function(action) {
    const map = tiled.activeAsset;
    if (!map.isTileMap) {
        tiled.alert("Not a tile map!");
        return;
    }

    var prc = new Process()
    prc.start(getGamePath(), [])
    for (let i = map.layerCount - 1; i >= 0; i--) {

        const layer = map.layerAt(i);
        if (layer.isObjectLayer) {

            for (let i = layer.objectCount - 1; i >= 0; i--) {
                const obj = layer.objectAt(i)
                let cln = layer.className ? layer.className : "" + obj.className ? `.${obj.className}` : ""
                if (cln === "") {
                    objectErrors.push(obj.id)
                    continue
                }
                prc.writeLine(cln)
                if (prc.readLine() !== "OK") {
                    if (!obj.className){
                        layerErrors.push(layer.id)
                        break
                    }
                    else objectErrors.push(obj.id)
                }
            }
        }
        else if (layer.isTileLayer) {
            prc.writeLine(layer.className)
            if (prc.readLine() !== "OK") layerErrors.push(layer.id)
        }
    }
    prc.writeLine("END")
})
checkMistakes.text = "Check map for mistakes"
checkMistakes.shortcut = "Ctrl+Shift+C"

var prevMistake = tiled.registerAction("PrevMistake", function(action) {
})
prevMistake.text = "Previous mistake"
prevMistake.shortcut = "Ctrl+Left"
prevMistake.enabled = false

var highlightCur = tiled.registerAction("HighlightCur", function(action) {
})
highlightCur.text = "Highlight current mistake"
highlightCur.shortcut = "Ctrl+Up"
highlightCur.enabled = false

var nextMistake = tiled.registerAction("NextMistake", function(action) {
})
nextMistake.text = "Next mistake"
nextMistake.shortcut = "Ctrl+Right"
nextMistake.enabled = false




tiled.extendMenu("Map", [
    { action: "MistakesCheck", before: "AutoMap" },
    { action: "PrevMistake"},
    { action: "HighlightCur"},
    { action: "NextMistake"},
    { separator: true }
]);


