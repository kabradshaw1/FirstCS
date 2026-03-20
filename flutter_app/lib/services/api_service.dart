// services/api_service.dart — HTTP client wrapper for SecondApi.
//
// This class is the single place in the Flutter app that knows how to talk to
// the backend. All screens will call methods on ApiService rather than making
// raw HTTP requests themselves. That way, if the base URL or request format
// ever changes, we only need to update one file.
//
// Architectural note:
//   main.dart  →  (future) UsersScreen  →  ApiService  →  SecondApi (ASP.NET)
//                                              ↕
//                                          User model

// `dart:convert` gives us jsonDecode() and jsonEncode() — Dart's built-in
// JSON tools (similar to System.Text.Json in C#).
import 'dart:convert';

// The `http` package (added in pubspec.yaml) wraps Dart's lower-level
// HttpClient with a friendlier API similar to fetch() in JavaScript or
// HttpClient in C#.
import 'package:http/http.dart' as http;

// Import our User model so we can return typed objects instead of raw Maps.
import '../models/user.dart';

// ---------------------------------------------------------------------------
// ApiService
// ---------------------------------------------------------------------------

class ApiService {
  // baseUrl points at our running SecondApi instance.
  // `static const` means the value belongs to the class (not an instance) and
  // is fixed at compile time — equivalent to `public const string` in C#.
  //
  // On a real device or when using an Android emulator you would change this
  // to your machine's LAN IP (e.g. 'http://192.168.1.x:5026'), because
  // 'localhost' inside an emulator refers to the emulator itself, not the
  // host machine.
  static const String baseUrl = 'http://localhost:5026';

  // ---------------------------------------------------------------------------
  // getUsers — GET /users
  // ---------------------------------------------------------------------------
  //
  // `Future<List<User>>` is the return type.
  //   • `Future` is Dart's equivalent of C#'s `Task` — it represents a value
  //     that will be available *eventually* (after the network round-trip).
  //   • `List<User>` is a typed list, like `List<User>` in C#.
  //
  // `async` marks the method as asynchronous so we can use `await` inside it,
  // exactly like `async/await` in C#.
  Future<List<User>> getUsers() async {
    // Uri.parse converts a string into a structured Uri object.
    // The http package requires a Uri, not a plain string.
    final uri = Uri.parse('$baseUrl/users');

    // `await` pauses execution here until the HTTP response arrives.
    // http.get() sends a GET request and returns an http.Response.
    try {
      final response = await http.get(uri);
      // response.statusCode is the HTTP status code (200, 404, 500, etc.).
      // 200 means OK — the request succeeded.
      if (response.statusCode == 200) {
        // response.body is the raw response string (JSON text).
        // jsonDecode() parses it into a Dart object.
        // The API returns a JSON array, so we cast to List<dynamic>.
        final List<dynamic> jsonList =
            jsonDecode(response.body) as List<dynamic>;

        // We map each raw JSON map to a typed User using User.fromJson().
        // `.map()` transforms every element; `.toList()` materialises the result.
        // This is equivalent to LINQ's `.Select(...).ToList()` in C#.
        return jsonList
            .map((json) => User.fromJson(json as Map<String, dynamic>))
            .toList();
      } else {
        // If the server returns a non-200 status we throw an Exception.
        // The calling widget can catch this and display an error message.
        throw Exception('Failed to load users (status ${response.statusCode})');
      }
    } catch (e) {
      throw Exception('Network error: $e');
    }
  }

  // ---------------------------------------------------------------------------
  // createUser — POST /users
  // ---------------------------------------------------------------------------
  //
  // Accepts a name and age, sends them to the API, and returns the newly
  // created User (including the id that the server assigned).
  Future<User> createUser(String name, int age) async {
    final uri = Uri.parse('$baseUrl/users');

    // http.post() sends a POST request.
    // `headers` tells the server we are sending JSON (Content-Type).
    // `body` is the serialised request payload.
    //   jsonEncode() converts a Map to a JSON string — the opposite of
    //   jsonDecode(). The server expects { "name": "...", "age": ... }.
    final response = await http.post(
      uri,
      headers: {'Content-Type': 'application/json'},
      body: jsonEncode({'name': name, 'age': age}),
    );

    // 201 Created is the conventional success code for POST endpoints that
    // create a new resource. SecondApi returns 201 with the new User in the
    // response body.
    if (response.statusCode == 201) {
      final Map<String, dynamic> json =
          jsonDecode(response.body) as Map<String, dynamic>;
      return User.fromJson(json);
    } else {
      throw Exception('Failed to create user (status ${response.statusCode})');
    }
  }
}
