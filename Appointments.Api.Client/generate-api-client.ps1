npx swagger-typescript-api --path ./swagger.json --output ./src --name index.ts --api-class-name AppointmentsApiClient --union-enums --add-readonly --module-name-first-tag
npm run build
npm run package
#npm link
