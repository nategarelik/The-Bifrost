# Contributing to UnityBridge (Bifrost)

Thank you for your interest in contributing! UnityBridge is an open-source, AI-powered Unity Editor tool. We welcome contributions of all kinds: bug fixes, new features, prompt templates, documentation, and more.

## Getting Started

- Fork this repository and clone your fork.
- Open the project in Unity 2021.3 or newer.
- All main code is under `Assets/Bifrost`.

## Coding Standards

- Follow Unity C# conventions (PascalCase for public, camelCase for private).
- Use [SerializeField] for inspector-exposed fields.
- Use Unity's built-in systems (AssetDatabase, EditorWindow, Selection, etc.).
- Add error handling and null checks.
- Use async/await for AI calls.

## Adding a New LLM Provider

- Implement the `IBifrostLLMProvider` interface in `Assets/Bifrost/Editor`.
- Add your provider to the `BifrostProvider` enum and update the settings UI.
- Test with the built-in connection test.

## Adding Prompt Templates

- Add `.txt` files to `Assets/Bifrost/Resources/Prompts/`.
- Use clear, descriptive names and document the template's purpose.

## Pull Requests

- Create a feature or bugfix branch from `main`.
- Ensure your code builds and passes any tests.
- Add or update documentation as needed.
- Open a pull request with a clear description of your changes.

## Reporting Issues

- Use [GitHub Issues](https://github.com/nategarelik/the-unitybridge/issues) for bugs, feature requests, or questions.

## Code of Conduct

- Be respectful and constructive.
- Help us build a welcoming, inclusive community.

Thank you for helping make UnityBridge better!
