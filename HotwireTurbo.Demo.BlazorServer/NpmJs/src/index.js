import * as Turbo from "@hotwired/turbo";
import { Application } from "@hotwired/stimulus"
import { definitionsFromContext } from "@hotwired/stimulus-webpack-helpers"

window.Stimulus = Application.start()
const context = require.context("./controllers", true, /\.js$/)
Stimulus.load(definitionsFromContext(context))


console.log("initialized turbo", Turbo)
console.log("initialized stimulus", window.Stimulus)