export class Guid {
	static random(): string {
		return (
			Guid.randomHex() +
			Guid.randomHex() +
			Guid.randomHex() +
			Guid.randomHex() +
			'-' +
			Guid.randomHex() +
			Guid.randomHex() +
			'-' +
			Guid.randomHex() +
			Guid.randomHex() +
			'-' +
			Guid.randomHex() +
			Guid.randomHex() +
			'-' +
			Guid.randomHex() +
			Guid.randomHex() +
			Guid.randomHex() +
			Guid.randomHex() +
			Guid.randomHex() +
			Guid.randomHex() +
			Guid.randomHex() +
			Guid.randomHex()
		);
	}

	private static randomHex(): string {
		return Math.floor(Math.random() * 16).toString(16);
	}
}
