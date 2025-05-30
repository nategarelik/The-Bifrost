# The UnityBridge (Bifrost)

**AI-powered Unity Editor tool for natural language game development.**

---

## Overview

UnityBridge (codename: Bifrost) is an advanced Unity Editor extension that enables you to create, modify, and manage complete 2D/3D games, systems, and levels using natural language. Powered by multi-provider LLM support, deep Unity integration, and best practices, UnityBridge empowers rapid, AI-driven game development directly inside the Unity Editor.

## Features

- 🧠 **Chat Interface:** Request scripts, assets, systems, or entire games via natural language.
- 🔌 **Multi-Provider AI:** Supports OpenAI, Anthropic, Gemini, OpenRouter, Hugging Face, and Local providers.
- ⚙️ **Settings System:** Global and per-project settings for API keys, models, and advanced LLM options (max tokens, temperature, timeout, penalties, custom headers).
- 📚 **Prompt Template Library:** Built-in templates for 2D/3D games, level design, ProBuilder, UI/UX, and Steam publishing. Integrated UI for browsing and inserting templates.
- 🏗️ **Unity Context Analyzer:** Injects project/scene context into every AI call for more relevant results.
- 🛠️ **Game System Generation:** AI outputs strict, validated JSON plans for scripts, prefabs, scenes, and UI, with robust fallback for malformed responses.
- 🧩 **Unity Project Manipulation:** Create/open/save scenes, generate prefabs, place assets, duplicate prefabs, and more. ProBuilder integration (if present).
- 🧑‍💻 **Onboarding & Help:** Friendly onboarding, tooltips, and a Steam-ready game guide panel.
- 🛡️ **Error Handling:** All operations are robust, with clear error messages and safe fallbacks.

## Installation

### Option 1: Unity Package Manager (Recommended)

1. Open Unity and go to **Window > Package Manager**
2. Click the **+** button and select **Add package from git URL...**
3. Enter:
   ```
   https://github.com/nategarelik/The-Bifrost.git?path=Assets/Bifrost
   ```
4. Click **Add**. The package will appear in your project under `Assets/Bifrost`.

### Option 2: Manual Import

- Download or clone this repo.
- Copy the `Assets/Bifrost` folder into your Unity project's `Assets/` directory.

## Usage

1. Open the tool via **Window > Bifrost AI Assistant**
2. Use the Chat tab to request scripts, systems, or assets in natural language.
3. Review and approve AI-generated plans before they are applied.
4. Explore the Settings and Prompt Library tabs for customization and advanced features.
5. Use the Steam Guide for a checklist to publish your game on Steam.

## Screenshots

_Coming soon!_

## Requirements

- Unity 2021.3 or newer
- Internet connection for AI features

## Support & Community

- [GitHub Issues](https://github.com/nategarelik/the-unitybridge/issues)
- (Optional) Discord/Community link

---

## Release Checklist: Steam-Ready Game

1. **Prepare your game for build:**
   - Finalize scenes, assets, and settings.
   - Test in the Unity Editor.
2. **Integrate Steamworks:**
   - Use Steamworks.NET or Facepunch.Steamworks for Steam API integration.
   - Implement achievements, cloud saves, etc. as needed.
3. **Set up your Steam app:**
   - Create your app on the [Steamworks dashboard](https://partner.steamgames.com/).
   - Fill out all required metadata and assets.
4. **Build and upload:**
   - Build your game for Windows/Mac/Linux as needed.
   - Use SteamPipe to upload builds to Steam.
5. **Test with Steam client:**
   - Download and run your game via Steam to verify integration.
6. **Follow Steam's release checklist:**
   - Complete all required checkboxes in the Steamworks dashboard.
   - Submit for review and schedule your release!

---

© 2024 Nate Garelik. MIT License.
