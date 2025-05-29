# Bifrost TODO (Updated)

## Completed

- [x] Settings UI: Advanced LLM Options (max tokens, temperature, timeout, etc. now exposed)
- [x] Tooltips, Help, and Onboarding (onboarding panel, tooltips in settings, welcome/help panel)
- [x] LLM Response Schema & Fallbacks (strict schema, validation, fallback logic implemented)
- [x] Providers & Advanced Provider Features (OpenAI, Anthropic, Gemini, OpenRouter, Hugging Face, Local; custom headers, advanced options)
- [x] PromptTemplateManager & Prompt Templates (categories, CRUD, templates for 2D/3D/Level/ProBuilder/UI/Steam)

## In Progress / Remaining

- [ ] UnityContextAnalyzer
  - [ ] Feed Unity context into every AI call (context not yet injected into prompt construction)
  - [ ] Further enhance context details if needed
- [ ] UnityProjectManager
  - [ ] ProBuilder integration (procedural geometry, mesh editing)
  - [ ] More advanced asset/scene/prefab operations
- [ ] Onboarding & UX
  - [ ] In-depth guides/tooltips (e.g., "How to get a Steam-ready game")
  - [ ] More contextual help in UI (beyond onboarding panel)
- [ ] Prompt Library UI Integration
  - [ ] Integrate prompt library UI into the main window for easy access and use

## Notes

- All LLM providers now use advanced options and custom headers.
- Prompt templates are scaffolded for all major categories.
- LLM response schema and fallback are enforced for all game system plans.
- Settings and onboarding are extensible for future guides and help content.
