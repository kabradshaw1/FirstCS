// models/user.dart — a Dart class that mirrors the User model in SecondApi.
//
// In C# (SecondApi/Model/Model.cs) the User record looks like:
//   record User(int Id, string Name, int Age);
//
// We need an equivalent in Dart so we can deserialise the JSON that comes back
// from GET /users and POST /users.

// ---------------------------------------------------------------------------
// The User class
// ---------------------------------------------------------------------------

// Dart classes are declared with the `class` keyword (same as C#).
// All fields are `final` here because a User value object should not change
// once it has been created — immutability prevents accidental mutation.
class User {
  // `final` means the field can only be set in the constructor and never again.
  // In Dart, `int` and `String` are non-nullable by default (no `?` suffix),
  // so the compiler will prevent us from accidentally passing null.
  final int id;
  final String name;
  final int age;

  // The `const` constructor allows Dart to create User instances at compile
  // time (useful for test fixtures). `required` means callers MUST supply
  // each named argument — similar to non-optional parameters in C#.
  const User({
    required this.id,
    required this.name,
    required this.age,
  });

  // ---------------------------------------------------------------------------
  // fromJson — factory constructor for deserialisation
  // ---------------------------------------------------------------------------
  //
  // A "factory constructor" (keyword `factory`) can return an existing object
  // or perform logic before creating one. We use it here to convert a raw
  // Map (the parsed JSON) into a typed User.
  //
  // `Map<String, dynamic>` is Dart's way of saying "a map whose keys are
  // strings and whose values can be anything". This matches what
  // `jsonDecode()` from `dart:convert` produces.
  //
  // The API sends back JSON like:
  //   { "id": 1, "name": "Alice", "age": 30 }
  // The keys match the C# record property names (lowercased by the serialiser).
  factory User.fromJson(Map<String, dynamic> json) {
    return User(
      // `as int` casts the dynamic value to int. If the JSON is malformed
      // this will throw a TypeError at runtime — good for catching API bugs
      // early during development.
      id: json['id'] as int,
      name: json['name'] as String,
      age: json['age'] as int,
    );
  }

  // ---------------------------------------------------------------------------
  // toJson — convert a User back to a Map for POST /users
  // ---------------------------------------------------------------------------
  //
  // When creating a new user we send:
  //   { "name": "Bob", "age": 25 }
  // Note: `id` is omitted because the API assigns it automatically (just like
  // an auto-increment primary key in a database).
  Map<String, dynamic> toJson() {
    return {
      'name': name,
      'age': age,
    };
  }

  // toString() is handy for debugging — print(user) will show the fields.
  // This is equivalent to overriding ToString() in C#.
  @override
  String toString() => 'User(id: $id, name: $name, age: $age)';
}
