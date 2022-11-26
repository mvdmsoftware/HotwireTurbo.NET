import { Controller } from "@hotwired/stimulus"

export default class extends Controller {
    static targets = ["name", "greeting"]

    greet() {
        const greeting = `Hello, ${this.name}!`
        this.greetingTarget.textContent = greeting
    }

    get name() {
        return this.nameTarget.value
    }
}