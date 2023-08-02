<script lang="ts">
	import { goto } from '$app/navigation';
	import { AuthApi, LoginWithEmailRequest } from '$lib/api/auth';
	import { Button, Input } from '$lib/components/forms';

	const authApi = new AuthApi();

	let email = '';
	let password = '';

	async function onSubmit(e: Event) {
		e.preventDefault();
		await authApi.loginWithEmailAsync(new LoginWithEmailRequest(email, password));
		goto('/portal');
	}
</script>

<div class="d-flex justify-content-center align-items-center p-5">
	<div class="card" style="width: 400px">
		<div class="card-body">
			<form method="post" on:submit={onSubmit}>
				<Input
					class="mb-4"
					label="Email"
					placeholder="email@address.com"
					type="email"
					bind:value={email}
				/>
				<Input
					class="mb-4"
					label="Password"
					placeholder="********"
					type="password"
					bind:value={password}
				/>
				<Button class="w-100" label="Login" type="submit" variant="primary" />
			</form>
		</div>
	</div>
</div>
